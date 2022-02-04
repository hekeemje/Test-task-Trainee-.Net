using System;
using System.Threading;

namespace TestTask.NetTraineeUkad
{
    class Program
    {
        static void Main(string[] args)
        {
            webCrawler webCrawler = new webCrawler();
            _ = webCrawler.startCrawler("https://github.com/");
            Thread.Sleep(1000); // Можно ли каким то способом подождать завершение метода сверху чтобы перейти дальше ?
            sitemapCrawler sitemap = new sitemapCrawler();
            sitemap.Parse();
        }
    }
}

