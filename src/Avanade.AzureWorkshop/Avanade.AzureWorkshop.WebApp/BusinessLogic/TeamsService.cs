using Avanade.AzureWorkshop.WebApp.Services;
using Avanade.AzureWorkshop.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.BusinessLogic
{
    public class TeamsService
    {
        private readonly CsvReader _csvReader;
        private readonly string teamsData = "Avanade.AzureWorkshop.WebApp.Resources.teams.csv";

        public TeamsService(CsvReader csvReader)
        {
            _csvReader = csvReader;
        }


        public HomePageViewModel GetHomePageData()
        {
            var groups = _csvReader.ReadTeams(teamsData).GroupBy(x => x.Group);

            var vm = new HomePageViewModel()
            {
                Groups = groups.Select(g => new GroupViewModel()
                {
                    GroupLetter = g.Key,
                    Teams = g.Select(t =>
                    new GroupTeamViewModel() {
                        Flag = t.Flag,
                        Name = t.Name
                    }).ToList()
                }).ToList()
            };


            return vm;
        }
    }
}