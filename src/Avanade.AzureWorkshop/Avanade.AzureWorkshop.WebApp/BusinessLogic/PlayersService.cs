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
        private readonly ImagesService _imagesService;

        public PlayersService(TeamsRepository teamsRepository, ImagesService imagesService)
        {
            _teamsRepository = teamsRepository;
            _imagesService = imagesService;
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
                              Position = player.Position,
                              PlayerId = player.RowKey
                          };

            return new TeamViewModel() { Players = players.ToList() };
        }

        public PlayerDetails GetPlayerDetails(string teamId, string playerId)
        {
            var player = _teamsRepository.FetchSinglePlayer(teamId, playerId);
            var images = _imagesService.SearchForImages(player.FullName);
            return new PlayerDetails()
            {
                Club = player.Club,
                DateOfBirth = player.DateOfBirth,
                FullName = player.FullName,
                Number = player.Number,
                Position = player.Position,
                PlayerId = player.RowKey,
                Images = images.ToList()
            };
        }
    }
}