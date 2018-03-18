using Avanade.AzureWorkshop.WebApp.Models.ServiceBusModels;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Avanade.AzureWorkshop.Topics
{
    public class Functions
    {
        private const string SubscriptionName = "webjobssubscription";

        public async Task ProcessGameMessage([ServiceBusTrigger(nameof(GameMessageModel), SubscriptionName)] GameMessageModel message, TextWriter textWriter)
        {
            await ProcessTopic(message, textWriter);
        }

        private async Task ProcessTopic<TTopic>(TTopic message, TextWriter textWriter) where TTopic : BaseMessageModel
        {
            await WriteMessage($"Processing topic message {typeof(TTopic).Name}. Body: {JsonConvert.SerializeObject(message)}", textWriter);
        }

        private static async Task WriteMessage(string message, TextWriter writer)
        {
            await writer.WriteLineAsync(message);
        }
    }
}
