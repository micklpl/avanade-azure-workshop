using Avanade.AzureWorkshop.WebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.Services
{
    public class CsvReader
    {
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


        public IEnumerable<Team> ReadTeams(string path)
        {
            var lines = ReadCsvResource(path).Split('\n');
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
    }
}