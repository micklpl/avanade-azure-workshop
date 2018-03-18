using Avanade.AzureWorkshop.WebApp.Models.ServiceBusModels;
using Avanade.AzureWorkshop.WebApp.Models.TableStorageModels;
using Avanade.AzureWorkshop.WebApp.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Avanade.AzureWorkshop.WebApp.BusinessLogic
{
    public class GamesService
    {
        private readonly TeamsRepository _teamsRepository;

        public GamesService(TeamsRepository teamsRepository)
        {
            _teamsRepository = teamsRepository;
        }

        public async Task SaveGameResult(GameMessageModel gameMessageModel)
        {
            await SaveGame(gameMessageModel);
            UpdateTeamPoints(gameMessageModel);
            UpdatePlayersGoals(gameMessageModel);
        }

        private async Task SaveGame(GameMessageModel gameMessageModel)
        {
            var game = new GameEntity(gameMessageModel.Group, Guid.NewGuid().ToString())
            {
                DateOfGame = gameMessageModel.DateOfGame,
                Team1Name = gameMessageModel.Team1Name,
                Team2Name = gameMessageModel.Team2Name,
                Team1Goals = gameMessageModel.Team1Goals,
                Team2Goals = gameMessageModel.Team2Goals,
            };

            await _teamsRepository.StoreGame(game);
        }

        private void UpdatePlayersGoals(GameMessageModel gameMessageModel)
        {
            var team1players = _teamsRepository.FetchPlayers(gameMessageModel.Team1Name.Replace(" ", "")).ToList();
            var team2players = _teamsRepository.FetchPlayers(gameMessageModel.Team2Name.Replace(" ", "")).ToList();

            foreach (var scorer in gameMessageModel.Team1Scorers)
            {
                var player = team1players.Find(x => x.FullName == scorer);
                if (player != null)
                {
                    player.Goals++;
                }
            }

            foreach (var scorer in gameMessageModel.Team2Scorers)
            {
                var player = team2players.Find(x => x.FullName == scorer);
                if (player != null)
                {
                    player.Goals++;
                }
            }

            _teamsRepository.UpdatePlayers(team1players);
            _teamsRepository.UpdatePlayers(team2players);
        }

        private void UpdateTeamPoints(GameMessageModel gameMessageModel)
        {
            var team1 = _teamsRepository.FetchTeam(gameMessageModel.Team1Name);
            var team2 = _teamsRepository.FetchTeam(gameMessageModel.Team2Name);

            team1.Games++;
            team2.Games++;

            if (gameMessageModel.Team1Goals > gameMessageModel.Team2Goals)
            {
                team1.Points += 3;
            }
            else if (gameMessageModel.Team1Goals < gameMessageModel.Team2Goals)
            {
                team2.Points += 3;
            }
            else
            {
                team1.Points++;
                team2.Points++;
            }

            _teamsRepository.UpdateTeam(team1);
            _teamsRepository.UpdateTeam(team2);
        }
    }
}