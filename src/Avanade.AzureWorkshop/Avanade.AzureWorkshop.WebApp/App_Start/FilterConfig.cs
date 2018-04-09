using Avanade.AzureWorkshop.WebApp.Filters;
using System.Web;
using System.Web.Mvc;

namespace Avanade.AzureWorkshop.WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AiHandleErrorAttribute());
            filters.Add(new CorrelationIdFilter());
        }
    }
}
