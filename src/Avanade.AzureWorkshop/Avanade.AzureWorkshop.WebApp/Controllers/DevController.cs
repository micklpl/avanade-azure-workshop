using Avanade.AzureWorkshop.WebApp.Models;
using Avanade.AzureWorkshop.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Avanade.AzureWorkshop.WebApp.Controllers
{
    public class DevController : Controller
    {
        private readonly CsvReader _csvReader;
        private readonly TeamsRepository _teamsRepository;

        public DevController(CsvReader csvReader, TeamsRepository teamsRepository)
        {
            _csvReader = csvReader;
            _teamsRepository = teamsRepository;
        }

        // https://github.com/projectkudu/kudu/wiki/Azure-runtime-environment
        public ActionResult Index()
        {
            ViewBag.SiteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
            ViewBag.Sku = Environment.GetEnvironmentVariable("WEBSITE_SKU");
            ViewBag.ComputeMode = Environment.GetEnvironmentVariable("WEBSITE_COMPUTE_MODE");
            ViewBag.HostName = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
            ViewBag.InstanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
            return View();
        }

        public async Task<ActionResult> FillStorage()
        {
            var players = _csvReader.ReadPlayers();
            var teams = _csvReader.ReadTeams();

            await _teamsRepository.StoreTeams(teams.Select(MapTeam));
            await _teamsRepository.StorePlayers(players.Select(MapPlayer));

            return RedirectToAction("Index");
        }

        private dynamic MapPlayer(Player player)
        {
            throw new NotImplementedException();
        }

        private dynamic MapTeam(Team player)
        {
            throw new NotImplementedException();
        }
    }
}