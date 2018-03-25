using Autofac;
using Avanade.AzureWorkshop.WebApp.BusinessLogic;
using Avanade.AzureWorkshop.WebApp.Services;

namespace Avanade.AzureWorkshop.Topics
{
    public class IocConfig
    {
        private static IContainer _container;

        public IocConfig()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<GamesService>();
            builder.RegisterType<TeamsRepository>();
            builder.RegisterType<MailgunService>();
            builder.RegisterType<NewsletterService>();
            builder.RegisterType<TelemetryService>();

            _container = builder.Build();
        }

        public IContainer GetConfiguredContainer()
        {
            return _container;
        }
    }
}
