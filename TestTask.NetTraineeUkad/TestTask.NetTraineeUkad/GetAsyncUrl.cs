using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestTask.NetTraineeUkad
{
    internal class GetAsyncUrl
    {
        private readonly HttpClient _client;
        private readonly Dictionary<string, int> urlsPing;

        public GetAsyncUrl()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36");
            urlsPing = new Dictionary<string, int>();
        }

        public async Task<Dictionary<string, int>> StartAsync(List<string> urls)
        {
            var tasks = new List<Task>();

            foreach (var item in urls)
            {
                tasks.Add(AsyncTime(item));
            }

            await Task.WhenAll(tasks);

            return urlsPing;
        }

        private async Task AsyncTime(string url)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode || response.Content == null || response.Content.Headers.ContentType.MediaType != "text/html")
            {
                Console.WriteLine($">> Invalid response for {url} <<\n");
                return;
            }

            var sw = new Stopwatch();
            sw.Restart();
            await _client.GetStringAsync(url);
            sw.Stop();

            urlsPing.Add(url, (int)sw.ElapsedMilliseconds);
        }
    }
}
