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

        public async Task CreateGames()
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("games");

            await table.CreateIfNotExistsAsync();
        }

        public IEnumerable<GameEntity> FetchGamesByGroup(string group)
        {
            var tableClient = GetClient();
            CloudTable table = tableClient.GetTableReference("games");
            var query = new TableQuery<GameEntity>()
                .Where(TableQuery.GenerateFilterCondition(nameof(GameEntity.Group), QueryComparisons.Equal, group));
            return table.ExecuteQuery(query).OrderBy(f => f.DateOfGame);
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
    }
}