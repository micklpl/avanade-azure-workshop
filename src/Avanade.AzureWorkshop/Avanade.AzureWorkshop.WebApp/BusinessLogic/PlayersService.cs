using Avanade.AzureWorkshop.WebApp.Services;
using Avanade.AzureWorkshop.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.BusinessLogic
{
    public class PlayersService
    {
        private readonly TeamsRepository _teamsRepository;
        private readonly ImagesService _imagesService;
        private readonly BinaryFilesRepository _binaryFilesRepository;

        public PlayersService(TeamsRepository teamsRepository, 
            ImagesService imagesService, BinaryFilesRepository binaryFilesRepository)
        {
            _teamsRepository = teamsRepository;
            _imagesService = imagesService;
            _binaryFilesRepository = binaryFilesRepository;
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

            return new PlayerDetails()
            {
                Club = player.Club,
                DateOfBirth = player.DateOfBirth,
                FullName = player.FullName,
                Number = player.Number,
                Position = player.Position,
                PlayerId = player.RowKey,
                Images = GetPlayerImages(player.FullName, player.RowKey, player.PartitionKey)
            };
        }

        private List<string> GetPlayerImages(string fullName, string playerId, string teamId)
        {
            if (_binaryFilesRepository.AnyFileExists(teamId, playerId))
            {
                return _binaryFilesRepository.GetBlobUrls(teamId, playerId);
            }

            var images = _imagesService.SearchForImages(fullName).ToList();

            foreach(var image in images)
            {
                byte[] data = _imagesService.DownloadImage(image);
                _binaryFilesRepository.SaveBlob(teamId, playerId, data);
            }

            return images;
        }
    }
}