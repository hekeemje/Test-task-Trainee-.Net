using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text;
using System.Data.Odbc;
using System.Threading.Tasks;
using System.Threading;

namespace TestTask.NetTraineeUkad
{
    class webCrawler
    {
        Dictionary<string, string> list = new Dictionary<string, string>();
        Dictionary<string, string> visitedUrl = new Dictionary<string, string>();

        internal async Task startCrawler(string url)
        {
            var checkUrl = url.Replace("https://", "");
            await Task.Run(() => proccessCrawler(url, checkUrl));
            visitedUrl.Add(url, "");

            foreach (var item in list)
            {
                try
                {
                    Console.WriteLine($"now try {item.Key}");
                    visitedUrl.Add(item.Key, "");
                    await Task.Run(() => proccessCrawler(item.Key, item.Key.Replace("https://", "")));
                    Console.WriteLine($"added new !!! {item.Key}");
                }
                catch
                {

                }
            }

            foreach (var item in list)
            {
                Console.WriteLine(item.Key);
            }
        }

        private async Task proccessCrawler(string url, string checkUrl)
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes("//a[@href]");

            foreach (var n in nodes)
            {
                string href = n.Attributes["href"].Value;
                try
                {
                    var absUrl = GetAbsoluteUrlString(url, href);
                    if (absUrl.Contains(checkUrl))
                    {
                        list.Add(absUrl, "");
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