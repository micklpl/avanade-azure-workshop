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
        private const int MaxGoalsInGame = 5;
        private static Random _rnd = new Random();

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

        public void PlayGame(string group)
        {
            var teamsInGroup = _teamsRepository.FetchTeamsByGroup(group).Where(x => x.Games < 3).ToList();
            var gamesInGroup = _teamsRepository.FetchGamesByGroup(group).ToList();

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