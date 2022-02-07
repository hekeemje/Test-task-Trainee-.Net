using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestTask.NetTraineeUkad
{
    class Program
    {
        static async Task Main()
        {
            try
            {
                Console.Write("Please, input url adress for crawling ( Example : https://github.com/ ) \nUrl : ");
                Uri url = new(Console.ReadLine());
                Console.WriteLine("Running...\n");

                //var url = "https://seoagilitytools.com/";

                using var webCrawler = new WebCrawler();
                var websiteUrls = await webCrawler.StartCrawler(url.ToString());

                var sitemap = new SitemapCrawler();
                var sitemapUrls = new List<string>();

                try
                {
                    var sitemapUrl = url + "sitemap.xml";
                    sitemapUrls = SitemapCrawler.Parse(sitemapUrl);
                }

                catch
                {
                    Console.WriteLine($"\nSitemap doesnt exist on {url}\n");
                }

                var outputInfo = new OutputInfo();
                await OutputInfo.OutputAllInfoAsync(websiteUrls, sitemapUrls);
            }
            catch
            {
                Console.WriteLine("Sorry, your link was incorrect.");
            }
        }
    }
}
