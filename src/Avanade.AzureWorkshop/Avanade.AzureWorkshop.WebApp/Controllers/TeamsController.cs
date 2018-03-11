using Avanade.AzureWorkshop.WebApp.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avanade.AzureWorkshop.WebApp.Controllers
{
    public class TeamsController : Controller
    {
        private readonly PlayersService _playersService;

        public TeamsController(PlayersService playersService)
        {
            _playersService = playersService;
        }

        // GET: Teams
        public ActionResult Index(string id)
        {
            return View(_playersService.GetTeamDetails(id));
        }
    }
}