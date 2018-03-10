using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avanade.AzureWorkshop.WebApp.Controllers
{
    public class DevController : Controller
    {
        // https://github.com/projectkudu/kudu/wiki/Azure-runtime-environment
        public ActionResult Index()
        {
            ViewBag.SiteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
            ViewBag.Sku = Environment.GetEnvironmentVariable("WEBSITE_SKU");
            ViewBag.ComputeMode = Environment.GetEnvironmentVariable("WEBSITE_COMPUTE_MODE");
            ViewBag.HostName = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
            ViewBag.InstanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
            return View();
        }
    }
}