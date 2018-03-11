using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.Models.TableStorageModels
{
    public class TeamEntity : TableEntity
    {
        public TeamEntity()
        {

        }

        public TeamEntity(string partition, string id)
        {
            PartitionKey = partition;
            RowKey = id;
        }

        public string Group { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Flag { get; set; }
        public int Points { get; set; }
    }
}