using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestTask.NetTraineeUkad
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var webCrawler = new webCrawler();
            var links = await webCrawler.startCrawler("https://github.com/");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine(string.Join(Environment.NewLine, links));
            sitemapCrawler sitemap = new sitemapCrawler();
            sitemap.Parse();
        }
    }
}

