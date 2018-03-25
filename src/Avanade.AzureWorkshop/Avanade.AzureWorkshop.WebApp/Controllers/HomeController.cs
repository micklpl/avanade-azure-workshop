using Avanade.AzureWorkshop.WebApp.BusinessLogic;
using Avanade.AzureWorkshop.WebApp.Services;
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
        private readonly TelemetryService _telemetryService;

        public string CorrelationId { get { return HttpContext.Items["correlationId"] as string; } }

        public HomeController(TeamsService teamsService, TelemetryService telemetryService)
        {
            _teamsService = teamsService;
            _telemetryService = telemetryService;
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
            _telemetryService.Log($"Playing game in group {group}", CorrelationId);
            _teamsService.PlayGame(group, CorrelationId);
            return RedirectToAction("Index");
        }
    }
}