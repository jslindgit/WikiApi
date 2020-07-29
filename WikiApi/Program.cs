using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WikiApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(100, 20);

            Console.Write("Article name: ");
            string articleName = Console.ReadLine();
            string queryURL = "https://en.wikipedia.org/w/api.php?action=query&format=json&list=search&srsearch=" + articleName;
            Console.WriteLine("Reading " + queryURL + "/ ...");

            Task<string> taskResult = GetHttpContent(queryURL);
            string content = taskResult.Result;            
            Console.WriteLine(content);
        }


        static async Task<string> GetHttpContent(string url)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            var content = await httpClient.GetStringAsync(url);
            return content;
        }
    }
}
