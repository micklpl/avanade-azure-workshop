using System.Runtime.Serialization;

namespace Avanade.AzureWorkshop.WebApp.Models.ServiceBusModels
{
    public class BaseMessageModel
    {
        public string CorrelationId { get; set; }
    }
}