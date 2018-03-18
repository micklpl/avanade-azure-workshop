using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.ViewModels
{
    public class ScorersViewModel
    {
        public string PlayerId { get; set; }
        public string TeamId { get; set; }
        public string FullName { get; set; }
        public int Goals { get; set; }
    }
}