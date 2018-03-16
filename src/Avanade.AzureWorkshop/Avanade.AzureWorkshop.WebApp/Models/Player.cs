using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.Models
{
    public class Player
    {
        public string TeamId { get; set; }
        public int? Number { get; set; }
        public string Position { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Club { get; set; }
    }
}