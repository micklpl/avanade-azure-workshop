using Autofac;
using Avanade.AzureWorkshop.WebApp.BusinessLogic;
using Avanade.AzureWorkshop.WebApp.Models.ServiceBusModels;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Avanade.AzureWorkshop.Topics
{
    public class Functions
    {
        private const string SubscriptionName = "webjobssubscription";
        private const string NewsletterTriggerName = "Newsletter";

        public async Task ProcessGameMessage([ServiceBusTrigger(nameof(GameMessageModel), SubscriptionName)] GameMessageModel message, TextWriter textWriter)
        {
            await ProcessTopic(message, textWriter);
        }

        public async Task ProcessNewsletter([ServiceBusTrigger(NewsletterTriggerName, SubscriptionName)] BrokeredMessage message, TextWriter textWriter)
        {
            await WriteMessage("Newsletter arrived", textWriter);
        }

        private async Task ProcessTopic<TTopic>(TTopic message, TextWriter textWriter) where TTopic : BaseMessageModel
        {
            using (var scope = Program.Container.BeginLifetimeScope())
            {
                try
                {
                    await WriteMessage($"Processing topic message {typeof(TTopic).Name}. Body: {JsonConvert.SerializeObject(message)}", textWriter);

                    var gamesService = scope.Resolve<GamesService>();
                    await gamesService.SaveGameResult(message as GameMessageModel);
                }
                catch (Exception ex)
                {
                    textWriter.WriteLine($"Unexpected error {ex.Message} {ex.StackTrace} {ex.InnerException}");
                    throw;
                }

            }
        }

        private static async Task WriteMessage(string message, TextWriter writer)
        {
            await writer.WriteLineAsync(message);
        }
    }
}
