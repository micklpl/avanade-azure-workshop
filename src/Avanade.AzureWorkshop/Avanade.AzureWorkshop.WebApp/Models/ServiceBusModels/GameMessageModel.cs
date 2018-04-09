using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Avanade.AzureWorkshop.WebApp.Models.ServiceBusModels
{
    public class GameMessageModel : BaseMessageModel
    {
        public string Group { get; set; }
        public string Team1Name { get; set; }
        public string Team2Name { get; set; }
        public int Team1Goals { get; set; }
        public int Team2Goals { get; set; }
        public IEnumerable<string> Team1Scorers { get; set; }
        public IEnumerable<string> Team2Scorers { get; set; }
        public DateTime DateOfGame { get; set; }
    }
}