using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Threading;

namespace TestTask.NetTraineeUkad
{
    internal class WebCrawler : IDisposable
    {
        private const int maxRequestCount = 10;
        private int _requestCount;
        private readonly HashSet<string> _urls;
        private readonly HttpClient _client;
        string _checkurl;

        public WebCrawler()
        {
            _urls = new HashSet<string>();
            _client = new HttpClient();
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36");
        }

        internal async Task<List <string>> startCrawler(string url)
        {
            _requestCount = 0;
            _urls.Clear();
            _urls.Add(url);

            var checkurl = url;
            _checkurl = checkurl.Replace("https://", "");

            await proccessCrawler(url, 2);

            return _urls.ToList();
        }

        private async Task proccessCrawler(string url, int depth)
        {
            if (Interlocked.Increment(ref _requestCount) > maxRequestCount)
            {
                return;
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode || response.Content == null || response.Content.Headers.ContentType.MediaType != "text/html")
            {
                Console.WriteLine($">> Invalid response <<");
                return;
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(await response.Content.ReadAsStringAsync());

            var links = htmlDocument.DocumentNode.SelectNodes("//a[@href]").Select(node => node.Attributes["href"].Value).Where(link => link.Length > 0 && !link.Contains("#") && !link.Contains("?"));

            var tasks = new List<Task>();

            foreach (var link in links)
            {
                var absUrl = GetAbsoluteUrlString(url, link);

                if (!absUrl.Contains(_checkurl))
                {
                    continue;
                }

                lock (_urls)
                {
                    if (!_urls.Add(absUrl))
                    {
                        continue;
                    }
                }

                if (depth > 0)
                {
                    tasks.Add(proccessCrawler(absUrl, depth - 1));
                }
            }

            await Task.WhenAll(tasks);
        }

        private string GetAbsoluteUrlString(string baseUrl, string url)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);

            if (!uri.IsAbsoluteUri)
                uri = new Uri(new Uri(baseUrl), uri);

            return uri.ToString();
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}