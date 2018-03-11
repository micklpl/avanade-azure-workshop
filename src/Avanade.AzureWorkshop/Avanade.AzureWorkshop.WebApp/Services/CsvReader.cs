using Avanade.AzureWorkshop.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.Services
{
    public class CsvReader
    {

        private readonly string playersData = "Avanade.AzureWorkshop.WebApp.Resources.players.csv";
        private readonly string teamsData = "Avanade.AzureWorkshop.WebApp.Resources.teams.csv";

        private string ReadCsvResource(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var names = assembly.GetManifestResourceNames();
            using (Stream stream = assembly.GetManifestResourceStream(path))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }


        public IEnumerable<Team> ReadTeams()
        {
            var lines = ReadCsvResource(teamsData).Split('\n');
            foreach (var line in lines)
            {
                if(string.IsNullOrEmpty(line)) yield break;
                var columns = line.Split(',');
                yield return new Team()
                {
                    Group = Convert.ToChar(columns[0]),
                    Name = columns[1],
                    Url = columns[2],
                    Flag = columns[3]
                };
            }
        }

        public IEnumerable<Player> ReadPlayers()
        {
            var lines = ReadCsvResource(playersData).Split('\n');
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) yield break;
                var columns = line.Split(',');

                yield return new Player()
                {
                    TeamId = columns[0],
                    Number = TryParseNullable(columns[1]),
                    FullName = columns[2],
                    Position = columns[3],
                    DateOfBirth = DateTime.ParseExact(columns[4], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Club = columns[5]
                };
            }
        }

        private int? TryParseNullable(string val)
        {
            int outValue;
            return int.TryParse(val, out outValue) ? (int?)outValue : null;
        }
    }
}