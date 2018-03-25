using Avanade.AzureWorkshop.WebApp.Models.ServiceBusModels;
using Avanade.AzureWorkshop.WebApp.Models.TableStorageModels;
using Avanade.AzureWorkshop.WebApp.Services;
using Avanade.AzureWorkshop.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Avanade.AzureWorkshop.WebApp.BusinessLogic
{
    public class TeamsService
    {
        private readonly TeamsRepository _teamsRepository;
        private readonly TopicService<GameMessageModel> _topicService;
        private readonly TelemetryService _telemetryService;

        private const int MaxGoalsInGame = 5;
        private static Random _rnd = new Random();        

        public TeamsService(TeamsRepository teamsRepository, 
            TopicService<GameMessageModel> topicService, TelemetryService telemetryService)
        {
            _teamsRepository = teamsRepository;
            _topicService = topicService;
            _telemetryService = telemetryService;
        }


        public HomePageViewModel GetHomePageData()
        {
            var groups = _teamsRepository.FetchTeams().GroupBy(x => x.Group);
            var scorers = _teamsRepository.FetchScorers();

            var vm = new HomePageViewModel()
            {
                Groups = groups.Select(g => new GroupViewModel()
                {
                    GroupLetter = Convert.ToChar(g.Key),
                    Teams = g.Select(t =>
                    new GroupTeamViewModel() {
                        Flag = t.Flag,
                        Games = t.Games,
                        Name = t.Name,
                        Points = t.Points
                    }).OrderByDescending(x => x.Points).ToList()
                }).ToList(),
                Scorers = scorers.Select(x => new ScorersViewModel
                {
                    Goals = x.Goals,
                    FullName = x.FullName,
                    PlayerId = x.RowKey,
                    TeamId = x.TeamId
                }).ToList()
            };


            return vm;
        }

        public void PlayGame(string group, string correlationId)
        {
            var teamsInGroup = _teamsRepository.FetchTeamsByGroup(group).Where(x => x.Games < 3).ToList();
            var gamesInGroup = _teamsRepository.FetchGamesByGroup(group).ToList();

            if (teamsInGroup.Count == 0) return;

            var index = _rnd.Next(teamsInGroup.Count);

            var team1Goals = _rnd.Next(MaxGoalsInGame);
            var team2Goals = _rnd.Next(MaxGoalsInGame);

            var team1Name = teamsInGroup[index].Name;
            teamsInGroup.RemoveAt(index);
            var team2Name = DrawOpponent(team1Name, teamsInGroup, gamesInGroup);

            var team1Players = _teamsRepository.FetchPlayers(team1Name.Replace(" ","")).ToList();
            var team2Players = _teamsRepository.FetchPlayers(team2Name.Replace(" ", "")).ToList();

            var team1Scorers = DrawScorers(team1Players, team1Goals).ToList();
            var team2Scorers = DrawScorers(team2Players, team2Goals).ToList();

            _telemetryService.Log($"New result generated {team1Name}:{team2Name} {team1Goals}:{team2Goals}", correlationId);

            var gameMessageModel = new GameMessageModel
            {
                Group = group,
                Team1Name = team1Name,
                Team2Name = team2Name,
                DateOfGame = DateTime.Now,
                Team1Goals = team1Goals,
                Team2Goals = team2Goals,
                Team1Scorers = team1Scorers,
                Team2Scorers = team2Scorers,
                CorrelationId = correlationId
            };

            _topicService.SendMessage(gameMessageModel);
        }

        public string DrawOpponent(string hostTeam, IList<TeamEntity> teams, IList<GameEntity> games)
        {
            foreach (var teamEntity in teams)
            {
                var game = games.FirstOrDefault(x => (x.Team1Name == hostTeam || x.Team2Name == hostTeam) && (x.Team1Name == teamEntity.Name || x.Team2Name == teamEntity.Name));
                if (game == null)
                {
                    return teamEntity.Name;
                }
            }

            throw new Exception($"Opponent for team {hostTeam} not found!");
        }

        public IEnumerable<string> DrawScorers(IList<PlayerEntity> players, int goals)
        {
            var scorers = new List<string>();

            for (var i = 0; i < goals; i++)
            {
                var index = _rnd.Next(players.Count);
                scorers.Add(players[index].FullName);
            }

            return scorers;
        }
    }
}