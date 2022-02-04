using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace TestTask.NetTraineeUkad
{
    internal class webCrawler
    {
        Dictionary<string, string> listUrls = new();
        List<string> visitedUrl = new List<string>();
        static object locker = new object();
        private readonly HttpClient _client;

        public webCrawler()
        {
            _client = new HttpClient();
        }

        internal async Task startCrawler(string url)
        {
            visitedUrl.Add(url);
            await proccessCrawler(url);

            foreach (var item in listUrls.Keys.ToList())
            {
                if (!visitedUrl.Contains(item))
                {
                    visitedUrl.Add(item);
                    lock (locker)
                    {
                        _ = proccessCrawler(item);
                    }
                }
                else
                {
                    Console.WriteLine("Found repeat.");
                }
            }

            _client.Dispose();

            foreach (var item in listUrls.Keys.ToList())
            {
                Console.WriteLine(item);
            }
        }

        private async Task proccessCrawler(string url)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode || response.Content == null || response.Content.Headers.ContentType.MediaType != "text/html")
            {
                Console.WriteLine($">> Invalid response <<");
                return;
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(await response.Content.ReadAsStringAsync());

            var links = htmlDocument.DocumentNode.SelectNodes("//a[@href]").Select(node => node.Attributes["href"].Value).Where(link => link.Length > 0 && !link.StartsWith("#"));

            foreach (var link in links)
            {
                try
                {
                    var absUrl = GetAbsoluteUrlString(url, link);
                    var checkUrl = url.Replace("https://", "");

                    if (absUrl.Contains(checkUrl) && !listUrls.Keys.ToList().Contains(absUrl))
                    {
                        listUrls.Add(absUrl, "");
                    }
                }
                catch
                {

                }
            }
        }

        private string GetAbsoluteUrlString(string baseUrl, string url)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);

            if (!uri.IsAbsoluteUri)
                uri = new Uri(new Uri(baseUrl), uri);

            return uri.ToString();
        }
    }
}