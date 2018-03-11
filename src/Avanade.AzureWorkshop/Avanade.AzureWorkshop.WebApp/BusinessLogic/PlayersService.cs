using Avanade.AzureWorkshop.WebApp.Services;
using Avanade.AzureWorkshop.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.BusinessLogic
{
    public class PlayersService
    {
        private readonly TeamsRepository _teamsRepository;

        public PlayersService(TeamsRepository teamsRepository)
        {
            _teamsRepository = teamsRepository;
        }

        public TeamViewModel GetTeamDetails(string teamId)
        {
            var players = from player in _teamsRepository.FetchPlayers(teamId)
                          where player.TeamId == teamId
                          select new PlayerDetails()
                          {
                              Club = player.Club,
                              DateOfBirth = player.DateOfBirth,
                              FullName = player.FullName,
                              Number = player.Number,
                              Position = player.Position
                          };

            return new TeamViewModel() { Players = players.ToList() };
        }
    }
}