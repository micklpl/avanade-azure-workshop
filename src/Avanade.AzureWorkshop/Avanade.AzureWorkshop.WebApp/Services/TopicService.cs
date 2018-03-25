using Avanade.AzureWorkshop.WebApp.Models.ServiceBusModels;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Configuration;

namespace Avanade.AzureWorkshop.WebApp.Services
{
    public class TopicService<TTopic> where TTopic : BaseMessageModel
    {
        private readonly NamespaceManager _namespaceManager;
        private readonly MessagingFactory _messagingFactory;
        private TopicClient _topicClient;
        private readonly TelemetryService _telemetryService;

        public TopicService(TelemetryService telemetryService)
        {
            var scheme = ConfigurationManager.AppSettings["serviceBusScheme"];
            var serviceName = ConfigurationManager.AppSettings["serviceBusServiceName"];
            var sharedAccessKeyName = ConfigurationManager.AppSettings["serviceBusSharedAccessKeyName"];
            var sharedAccessKeyValue = ConfigurationManager.AppSettings["serviceBusSharedAccessKeyValue"];

            var uri = ServiceBusEnvironment.CreateServiceUri(scheme, serviceName, string.Empty);
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(sharedAccessKeyName, sharedAccessKeyValue);

            _namespaceManager = new NamespaceManager(uri, tokenProvider);
            _messagingFactory = MessagingFactory.Create(uri, tokenProvider);

            _telemetryService = telemetryService;
        }

        private void SendMessageToServiceBus(BrokeredMessage brokeredMessage)
        {
            try
            {
                _topicClient.Send(brokeredMessage);
                _telemetryService.Log("Succesfully send message", brokeredMessage.CorrelationId);
            }
            catch (Exception ex)
            {
                _telemetryService.Log("Failed to send message, " + ex.Message, brokeredMessage.CorrelationId);
            }
        }

        private void CreateAssetIfNotExists(string topicName)
        {
            if (!_namespaceManager.TopicExists(topicName))
            {
                _namespaceManager.CreateTopic(topicName);
            }

            _topicClient = _messagingFactory.CreateTopicClient(topicName);
        }

        private static BrokeredMessage CreateBrokeredMessage(TTopic message, DateTime? scheduledTime = default(DateTime?))
        {
            var brokeredMessage = new BrokeredMessage(message);

            if (scheduledTime != null)
            {
                brokeredMessage.ScheduledEnqueueTimeUtc = scheduledTime.Value;
            }
            return brokeredMessage;
        }

        public void SendMessage(TTopic message, DateTime? scheduledTime = default(DateTime?)) 
        {
            CreateAssetIfNotExists(message.GetType().Name);

            var brokeredMessage = CreateBrokeredMessage(message, scheduledTime);

            SendMessageToServiceBus(brokeredMessage);

        }
    }
}