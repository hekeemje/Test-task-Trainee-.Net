using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text;
using System.Data.Odbc;
using System.Threading.Tasks;

namespace TestTask.NetTraineeUkad
{
    class Program
    {
        static void Main(string[] args)
        {
            webCrawler webCrawler = new webCrawler();
            webCrawler.startCrawler("https://github.com/");
            Console.ReadLine();
        }
    }
}

