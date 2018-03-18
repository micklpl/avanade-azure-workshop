using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.ViewModels
{
    public class GroupViewModel
    {
        public char GroupLetter { get; set; }
        public List<GroupTeamViewModel> Teams { get; set; }
    }

    public class GroupTeamViewModel
    {
        public string Id { get { return Name.Replace(" ", ""); } }
        public string Name { get; set; }
        public string Flag { get; set; }
        public int Games { get; set; }
        public int Points { get; set; }
    }
}