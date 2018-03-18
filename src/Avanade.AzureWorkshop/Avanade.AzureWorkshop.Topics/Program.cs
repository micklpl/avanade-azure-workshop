using Microsoft.Azure.WebJobs;

namespace Avanade.AzureWorkshop.Topics
{
    internal class Program
    {
        private static void Main()
        { 
            var config = new JobHostConfiguration();
            config.UseServiceBus();
            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
