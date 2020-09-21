using Newtonsoft.Json.Linq;
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
                        
            Console.Write("Wikipedia title/link: ");
            string pageTitle = Console.ReadLine();
            if (pageTitle.Contains("/wiki/"))
            {
                pageTitle = pageTitle.Split(new string[] { "/wiki/" }, StringSplitOptions.None).Last();
            }
             
            string searchURL = "https://en.wikipedia.org/w/api.php?action=query&format=json&list=search&srsearch=" + pageTitle;
            Console.WriteLine("Reading " + searchURL + "/ ...");

            Task<string> taskResult = GetHttpContent(searchURL);
            string content = taskResult.Result;
            Console.WriteLine(content);
            JObject searchData = JObject.Parse(content);
            
            int totalHits = Int32.Parse(searchData["query"]["searchinfo"]["totalhits"].ToString());
            if (totalHits > 0)
            {
                string pageID = searchData["query"]["search"][0]["pageid"].ToString();
                Console.WriteLine("pageID=" + pageID);
                string queryURL = "https://en.wikipedia.org/w/api.php?action=query&prop=extracts&exintro&explaintext&pageids=" + pageID + "&format=json";
                string queryContent = GetHttpContent(queryURL).Result;
                JObject queryData = JObject.Parse(queryContent);
                string article = queryData["query"]["pages"][pageID]["extract"].ToString();

                article = article.Replace("</p><p>", "\r\n");

                //Console.WriteLine(article);

                Console.ReadKey();

                //System.IO.File.WriteAllText(@"C:\temp\" + pageID + ".txt", article);
            }
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
