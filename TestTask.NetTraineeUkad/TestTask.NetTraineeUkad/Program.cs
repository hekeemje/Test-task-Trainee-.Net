using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestTask.NetTraineeUkad
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.Write("Please, input url adress for crawling ( Example : https://github.com/ ) \nUrl : ");
            //string url = Console.ReadLine();

            var url = "https://seoagilitytools.com/";

            using var webCrawler = new WebCrawler();
            try
            {
                var websiteUrls = await webCrawler.startCrawler(url);

                Console.WriteLine("Running...\n");


                SitemapCrawler sitemap = new SitemapCrawler();
                List<string> sitemapUrls = new List<string>();

                OutputInfo outputInfo = new OutputInfo();

                try
                {
                    var sitemapUrl = url + "sitemap.xml";
                    sitemapUrls = sitemap.Parse(sitemapUrl);

                    outputInfo.byWebsite(websiteUrls, sitemapUrls);
                    outputInfo.bySitemap(websiteUrls, sitemapUrls);
                }

                catch
                {
                    Console.WriteLine($"\nSitemap doesnt exist on {url}\n");
                }

                if (sitemapUrls.Count == 0)
                {
                    outputInfo.checkPingAndSort(websiteUrls, websiteUrls.Count, sitemapUrls.Count);
                }
                else
                {
                    List<string> mergedUrls = outputInfo.mergeUrls(websiteUrls, sitemapUrls);

                    outputInfo.checkPingAndSort(mergedUrls, websiteUrls.Count, sitemapUrls.Count);
                }
            }
            catch
            {
                Console.WriteLine("Sorry, your link was incorrect.");
            }
        }
    }
}

