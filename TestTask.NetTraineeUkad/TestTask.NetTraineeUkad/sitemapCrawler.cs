using System;
using System.Net;
using System.Xml;

namespace TestTask.NetTraineeUkad
{
    internal class sitemapCrawler
    {
        public void Parse()
        {
            string sitemapURL = "https://seoagilitytools.com/sitemap.xml";
            WebClient wc = new WebClient();
            string sitemapString = wc.DownloadString(sitemapURL);
            XmlDocument urldoc = new XmlDocument();
            urldoc.LoadXml(sitemapString);
            XmlNodeList xmlSitemapList = urldoc.GetElementsByTagName("url");

            foreach (XmlNode node in xmlSitemapList)
            {
                if (node["loc"] != null)
                {
                    Console.WriteLine("Url sitemap example : " + node["loc"].InnerText);
                }
            }
        }
    }
}
