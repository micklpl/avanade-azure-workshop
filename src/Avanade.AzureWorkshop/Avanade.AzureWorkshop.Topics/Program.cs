using Autofac;
using Microsoft.Azure.WebJobs;

namespace Avanade.AzureWorkshop.Topics
{
    internal class Program
    {
        public static IContainer Container { get; private set; }

        private static void Main()
        { 
            var container = new IocConfig();
            Container = container.GetConfiguredContainer();

            var config = new JobHostConfiguration();
            config.UseServiceBus();
            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
