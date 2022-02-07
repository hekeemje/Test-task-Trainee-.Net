using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.NetTraineeUkad
{
    internal class OutputInfo
    {
        internal static async Task OutputAllInfoAsync(List<string> websiteUrls, List<string> sitemapUrls)
        {
            ByWebsite(websiteUrls, sitemapUrls);
            BySitemap(websiteUrls, sitemapUrls);

            if (sitemapUrls.Count == 0)
            {
                await CheckPingAndSortAsync(websiteUrls, websiteUrls.Count, sitemapUrls.Count);
            }
            else
            {
                var mergedUrls = MergeUrls(websiteUrls, sitemapUrls);

                await CheckPingAndSortAsync(mergedUrls, websiteUrls.Count, sitemapUrls.Count);
            }
        }

        private static void ByWebsite(List<string> websiteUrls, List<string> sitemapUrls)
        {
            Console.WriteLine("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml\n");

            var byWebsite = new List<string>();

            foreach (var item in websiteUrls)
            {
                if (!sitemapUrls.Contains(item))
                    byWebsite.Add(item);
            }

            if (byWebsite.Count == 0)
            {
                Console.WriteLine($"Nothing :)");
            }
            else
            {
                var i = 1;

                foreach (var item in byWebsite)
                {
                    Console.WriteLine($"{i++} - {item}");
                }
            }
        }

        private static void BySitemap(List<string> websiteUrls, List<string> sitemapUrls)
        {
            Console.WriteLine("\nUrls FOUNDED IN SITEMAP.XML but not founded after crawling a web site\n");

            var bySitemap = new List<string>();

            foreach (var item in sitemapUrls)
            {
                if (!websiteUrls.Contains(item))
                {
                    bySitemap.Add(item);
                }
            }

            if (bySitemap.Count == 0)
            {
                Console.WriteLine("0 - Nothing\n");
            }
            else
            {
                var i = 1;

                foreach (var item in bySitemap)
                {
                    Console.WriteLine($"{i++} - {item}");
                }
            }
        }

        private static List<string> MergeUrls(List<string> websiteUrls, List<string> sitemapUrls)
        {
            var mergedUrls = new List<string>();

            foreach (var item in websiteUrls)
            {
                mergedUrls.Add(item);
            }

            foreach (var item in sitemapUrls)
            {
                if (!mergedUrls.Contains(item))
                {
                    mergedUrls.Add(item);
                }
            }

            return mergedUrls;
        }

        private static async Task CheckPingAndSortAsync(List<string> allUrls, int websiteCount, int sitemapCount)
        {
            var getAsyncTime = new GetAsyncUrl();

            var urlsPing = await getAsyncTime.StartAsync(allUrls);

            var sortedDict = from entry in urlsPing orderby entry.Value ascending select entry;

            Console.WriteLine("Timing :\n");

            var i = 1;

            foreach (var item in sortedDict)
            {
                Console.WriteLine($"{i++} - {item.Key} = {item.Value} ms");
            }

            Console.WriteLine($"\nUrls(html documents) found after crawling a website : {websiteCount}");
            Console.WriteLine($"Urls found in sitemap : {sitemapCount}");
        }
    }
}