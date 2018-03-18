using Avanade.AzureWorkshop.WebApp.Models.TableStorageModels;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.Services
{
    public class TeamsRepository
    {
        private CloudTableClient GetClient()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["storageConnectionString"]);
            return storageAccount.CreateCloudTableClient();
        }

        public void UpdateTeam(TeamEntity team)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("teams");

            table.Execute(TableOperation.Replace(team));
        }

        public void UpdatePlayers(IEnumerable<PlayerEntity> players)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("players");

            TableBatchOperation batchOperation = new TableBatchOperation();

            foreach (var player in players)
            {
                batchOperation.InsertOrReplace(player);
            }

            table.ExecuteBatch(batchOperation);
        }

        public async Task StoreTeams(IEnumerable<TeamEntity> teams)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("teams");

            await table.CreateIfNotExistsAsync();

            TableBatchOperation batchOperation = new TableBatchOperation();

            foreach(var team in teams)
            {
                batchOperation.Insert(team);
            }

            table.ExecuteBatch(batchOperation);
        }

        public async Task StorePlayers(IEnumerable<PlayerEntity> players)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("players");

            await table.CreateIfNotExistsAsync();            

            foreach (var group in players.GroupBy(p => p.PartitionKey))
            {
                foreach(var player in group)
                {
                    TableBatchOperation batchOperation = new TableBatchOperation();
                    batchOperation.Insert(player);
                    table.ExecuteBatch(batchOperation);
                }                
            }            
        }

        public async Task StoreGame(GameEntity game)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("games");

            await table.CreateIfNotExistsAsync();

            TableBatchOperation batchOperation = new TableBatchOperation();
            batchOperation.Insert(game);
            table.ExecuteBatch(batchOperation);
        }

        public IEnumerable<GameEntity> FetchGamesByGroup(string group)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("games");
            var query = new TableQuery<GameEntity>()
                .Where(TableQuery.GenerateFilterCondition(nameof(GameEntity.Group), QueryComparisons.Equal, group));
            return table.ExecuteQuery(query).OrderBy(f => f.DateOfGame);
        }

        public TeamEntity FetchTeam(string teamName)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("teams");
            var query = new TableQuery<TeamEntity>()
                .Where(TableQuery.GenerateFilterCondition(nameof(TeamEntity.Name), QueryComparisons.Equal, teamName));
            return table.ExecuteQuery(query).FirstOrDefault();
        }

        public IEnumerable<TeamEntity> FetchTeams()
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("teams");
            var query = new TableQuery<TeamEntity>();
            return table.ExecuteQuery(query).OrderBy(f => f.Group);
        }

        public IEnumerable<TeamEntity> FetchTeamsByGroup(string group)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("teams");
            var query = new TableQuery<TeamEntity>()
                .Where(TableQuery.GenerateFilterCondition(nameof(TeamEntity.Group), QueryComparisons.Equal, group));
            return table.ExecuteQuery(query).OrderBy(f => f.Name);
        }

        public IEnumerable<PlayerEntity> FetchPlayers(string teamId)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("players");
            var query = new TableQuery<PlayerEntity>()
                                .Where(TableQuery.GenerateFilterCondition(nameof(PlayerEntity.PartitionKey), QueryComparisons.Equal, teamId));
            return table.ExecuteQuery(query).OrderBy(f => f.Number);
        }

        public PlayerEntity FetchSinglePlayer(string teamId, string playerId)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("players");
            var retrieveOperation = TableOperation.Retrieve<PlayerEntity>(teamId, playerId);
            return table.Execute(retrieveOperation).Result as PlayerEntity;
        }

        public IEnumerable<PlayerEntity> FetchScorers()
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("players");
            var query = new TableQuery<PlayerEntity>()
                .Where(TableQuery.GenerateFilterConditionForInt(nameof(PlayerEntity.Goals), QueryComparisons.GreaterThan, 0));
            return table.ExecuteQuery(query).OrderByDescending(f => f.Goals);
        }
    }
}