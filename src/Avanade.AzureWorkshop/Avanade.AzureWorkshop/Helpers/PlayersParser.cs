using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avanade.AzureWorkshop.Helpers
{
    public static class PlayersParser
    {
        public static void Run()
        {
            var teams = File.ReadAllLines(@"teams.csv");
            var builder = new StringBuilder();

            foreach (var team in teams)
            {
                if (string.IsNullOrEmpty(team)) break;
                var columns = team.Split(',');
                var teamId = columns[1].Replace(" ", "");
                var teamDetailsUrl = columns[2];
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(teamDetailsUrl);

                var currentSquadSpan = doc.DocumentNode.SelectSingleNode("//span[@id='Current_squad']");
                if (currentSquadSpan == null) continue;
                var currentSquad = currentSquadSpan.ParentNode;
                HtmlNode nextNode = currentSquad.NextSibling;
                while(nextNode.Name != "table")
                {
                    nextNode = nextNode.NextSibling;
                }

                var table = nextNode;

                foreach(var row in table.SelectNodes(".//tr").Skip(1))
                {
                    var td = row.SelectNodes(".//td");
                    if (row.ChildNodes.Count < 7) continue;

                    var number = td[0].InnerText;
                    var position = td[1].SelectSingleNode(".//a").InnerText;
                    var name = row.SelectSingleNode(".//th").FirstChild.InnerText.Replace(",", "");
                    var dateOfBirth = row.SelectSingleNode(".//span[@class='bday']").InnerText;
                    var club = row.SelectNodes(".//a").Last().InnerText;

                    builder.Append($"{teamId},{number},{name},{position},{dateOfBirth},{club}\n");
                }
            }

            File.WriteAllText("players.csv", builder.ToString());
        }
    }
}
