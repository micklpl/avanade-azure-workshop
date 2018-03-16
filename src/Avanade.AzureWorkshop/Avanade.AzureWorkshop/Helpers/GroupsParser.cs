using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace Avanade.AzureWorkshop.Helpers
{
    public static class GroupsParser
    {
        public static void Run()
        {
            var groupsUrl = @"https://en.wikipedia.org/wiki/2018_FIFA_World_Cup";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(groupsUrl);

            var groups = doc.DocumentNode.SelectNodes("//table")
                                         .Where(t => t.Attributes.Any(a => a.Value == "wikitable"))
                                         .Where(t => t.InnerHtml.Contains("FIFA_World_Cup_Group_"))
                                         .ToList();
            var sb = new StringBuilder();

            for (int i = 0; i < groups.Count(); i++)
            {
                var letter = (char)(i + 'A');
                var spans = groups[i].SelectNodes(".//td").Where(t => t.ChildNodes.Any(c => c.Name == "span"));
                foreach(var span in spans)
                {
                    var team = span.SelectSingleNode(".//a");
                    var name = team.InnerText;
                    var url = @"https://en.wikipedia.org" + team.Attributes["href"].Value;
                    var flag = span.SelectSingleNode(".//img").Attributes["src"].Value;
                    byte[] flagBytes = new WebClient().DownloadData("https:" + flag);
                    sb.Append($"{letter},{name},{url},{Convert.ToBase64String(flagBytes)}\n");
                }
            }

            File.WriteAllText("teams.csv", sb.ToString());

        }
    }
}
