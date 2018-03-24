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
            await ProcessMessage(textWriter, message, async (scope, model) =>
            {
                var gamesService = scope.Resolve<GamesService>();
                await gamesService.SaveGameResult(message);
            });
        }

        public async Task ProcessNewsletter([ServiceBusTrigger(NewsletterTriggerName, SubscriptionName)] BrokeredMessage message, TextWriter textWriter)
        {
            await ProcessMessage(textWriter, new BaseMessageModel(), async (scope, model) =>
            {
                await WriteMessage("Newsletter arrived", textWriter);
            });
        }

        private static async Task ProcessMessage<TMessage>(TextWriter textWriter, TMessage message, Func<ILifetimeScope, TMessage, Task> action)
            where TMessage : BaseMessageModel
        {
            using (var scope = Program.Container.BeginLifetimeScope())
            {
                await WriteMessage($"Processing topic message {typeof(TMessage).Name}. Body: {JsonConvert.SerializeObject(message)}", textWriter);

                try
                {
                    await action(scope, message);
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
