using Avanade.AzureWorkshop.WebApp.Services;
using System.Threading.Tasks;

namespace Avanade.AzureWorkshop.WebApp.BusinessLogic
{
    public class NewsletterService
    {
        private readonly MailgunService _mailgunService;

        public NewsletterService(MailgunService mailgunService)
        {
            _mailgunService = mailgunService;
        }

        public async Task SendNewsletter()
        {
            await Task.FromResult<object>(_mailgunService.SendEmail());
        }
    }
}