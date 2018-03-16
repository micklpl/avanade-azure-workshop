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
        private readonly TeamsRepository _teamsRepository;

        public TeamsService(TeamsRepository teamsRepository)
        {
            _teamsRepository = teamsRepository;
        }


        public HomePageViewModel GetHomePageData()
        {
            var groups = _teamsRepository.FetchTeams().GroupBy(x => x.Group);

            var vm = new HomePageViewModel()
            {
                Groups = groups.Select(g => new GroupViewModel()
                {
                    GroupLetter = Convert.ToChar(g.Key),
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