using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.ViewModels
{
    public class HomePageViewModel
    {
        public List<GroupViewModel> Groups { get; set; }
        public List<ScorersViewModel> Scorers { get; set; }
    }
}