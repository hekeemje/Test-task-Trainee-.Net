using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestTask.NetTraineeUkad
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Please, input url adress for crawling ( Example : https://github.com/ ) \nUrl : ");
            string url = Console.ReadLine();
            Console.WriteLine("Running...\n");

            //var url = "https://seoagilitytools.com/";

            try
            {
                using var webCrawler = new WebCrawler();
                var websiteUrls = await webCrawler.startCrawler(url);

                var sitemap = new SitemapCrawler();
                var sitemapUrls = new List<string>();

                try
                {
                    var sitemapUrl = url + "sitemap.xml";
                    sitemapUrls = sitemap.Parse(sitemapUrl);
                }

                catch
                {
                    Console.WriteLine($"\nSitemap doesnt exist on {url}\n");
                }

                var outputInfo = new OutputInfo();
                await outputInfo.OutputAllInfoAsync(websiteUrls, sitemapUrls);
            }
            catch
            {
                Console.WriteLine("Sorry, your link was incorrect.");
            }
        }
    }
}

