using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Avanade.AzureWorkshop.WebApp.Models.TableStorageModels
{
    public class GameEntity : TableEntity
    {
        public GameEntity()
        {
            
        }

        public GameEntity(string group, string id)
        {
            PartitionKey = group;
            RowKey = id;
        }

        public string Group { get { return PartitionKey; } }
        public string Team1Name { get; set; }
        public string Team2Name { get; set; }
        public int Team1Goals { get; set; }
        public int Team2Goals { get; set; }
        public DateTime DateOfGame { get; set; }
    }
}