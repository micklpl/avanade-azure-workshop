using Avanade.AzureWorkshop.WebApp.BusinessLogic;
using Avanade.AzureWorkshop.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avanade.AzureWorkshop.WebApp.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ImagesService _imagesService;
        private readonly PlayersService _playersService;

        public TeamsController(PlayersService playersService, ImagesService imagesService)
        {
            _playersService = playersService;
            _imagesService = imagesService;
        }

        [Route("/Teams/{id}")]
        public ActionResult Index(string id)
        {
            var teamVm = _playersService.GetTeamDetails(id);
            teamVm.TeamId = id;
            return View(teamVm);
        }

        [Route("/Teams/{id}/Players")]
        public ActionResult PlayerDetails(string id, string playerId)
        {
            if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(playerId))
            {
                RedirectToAction("Index", "Home");
            }
            return View(_playersService.GetPlayerDetails(id, playerId));
        }
    }
}