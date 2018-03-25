using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avanade.AzureWorkshop.WebApp.Filters
{
    public class CorrelationIdFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var correlationId = Guid.NewGuid().ToString().Replace("-", "");
            filterContext.HttpContext.Items["correlationId"] = correlationId;
            filterContext.HttpContext.Response.Headers.Add("X-Correlation-Id", correlationId);
            base.OnActionExecuting(filterContext);
        }
    }
}