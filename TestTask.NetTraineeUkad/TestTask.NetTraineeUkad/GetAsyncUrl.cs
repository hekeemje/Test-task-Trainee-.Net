using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestTask.NetTraineeUkad
{
    class GetAsyncUrl
    {
        private readonly HttpClient _client;

        public GetAsyncUrl()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36");
        }

        public async Task<int> showAsyncTime(string url)
        {
            var sw = new Stopwatch();
            sw.Restart();
            await _client.GetStringAsync(url);
            sw.Stop();

            return (int)sw.ElapsedMilliseconds;
        }

    }
}
