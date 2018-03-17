using Avanade.AzureWorkshop.WebApp.BusinessLogic;
using Avanade.AzureWorkshop.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avanade.AzureWorkshop.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly TeamsService _teamsService;

        public HomeController(TeamsService teamsService)
        {
            _teamsService = teamsService;
        }

        public ActionResult Index()
        {
            ViewBag.MachineId = Environment.MachineName;
            return View(_teamsService.GetHomePageData());
        }

        public ActionResult Music()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("/PlayGame/{group}")]
        public ActionResult PlayGame(string group)
        {
            _teamsService.PlayGame(group);
            return RedirectToAction("Index");
        }
    }
}