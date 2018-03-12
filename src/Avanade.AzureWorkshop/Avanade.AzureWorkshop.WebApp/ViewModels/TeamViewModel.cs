using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.ViewModels
{
    public class TeamViewModel
    {
        public string TeamId { get; set; }
        public List<PlayerDetails> Players { get; set; }
    }

    public class PlayerDetails
    {
        public string PlayerId { get; set; }
        public int? Number { get; set; }
        public string Position { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { private get; set; }
        public int Age { get { return DateTime.Now.Year - DateOfBirth.Year; } }
        public string Club { get; set; }
        public List<string> Images { get; set; }
    }
}