using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.Services
{
    public class ImagesService
    {
        const string pattern = "\"contentUrl\": \"(.*?)\"";
        const int maxMatches = 3;

        private string accessKey = ConfigurationManager.AppSettings["bingImagesSearchKey"];
        const string uriBase = @"https://api.cognitive.microsoft.com/bing/v7.0/images/search";

        public IEnumerable<string> SearchForImages(string searchQuery)
        {
            var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(searchQuery);
            WebRequest request = HttpWebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;
            HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            int i = 0;
            foreach(Match match in Regex.Matches(json, pattern))
            {
                if (i++ == maxMatches) break;
                yield return match.Groups[1].Value;
            }
        }
    }
}