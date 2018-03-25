using System.Runtime.Serialization;

namespace Avanade.AzureWorkshop.WebApp.Models.ServiceBusModels
{
    [DataContract]
    public class BaseMessageModel
    {
        [DataMember]
        public string CorrelationId { get; set; }
    }
}