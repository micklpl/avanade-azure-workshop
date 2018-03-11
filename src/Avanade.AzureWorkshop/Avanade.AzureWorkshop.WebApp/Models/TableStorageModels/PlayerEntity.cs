using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.Models.TableStorageModels
{
    public class PlayerEntity : TableEntity
    {
        public PlayerEntity(string teamId, string id)
        {
            PartitionKey = teamId;
            RowKey = id;
        }

        public string TeamId { get { return PartitionKey; } }
        public int? Number { get; set; }
        public string Position { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Club { get; set; }
        public int Goals { get; set; }
        public string Thumbnail { get; set; }
    }
}