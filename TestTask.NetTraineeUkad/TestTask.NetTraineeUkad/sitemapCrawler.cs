using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace TestTask.NetTraineeUkad
{
    internal class SitemapCrawler
    {
        internal static List<string> Parse(string sitemapURL)
        {
            var wc = new WebClient();
            var sitemapString = wc.DownloadString(sitemapURL);
            var urldoc = new XmlDocument();
            urldoc.LoadXml(sitemapString);
            var xmlSitemapList = urldoc.GetElementsByTagName("url");

            var sitemapUrls = new List<string>();

            foreach (XmlNode node in xmlSitemapList)
            {
                if (node["loc"] != null)
                {
                    sitemapUrls.Add(node["loc"].InnerText);
                }
            }

            return sitemapUrls;
        }
    }
}
