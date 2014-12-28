using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PartyService
{
	public class WebApiApplication : System.Web.HttpApplication
	{
        public static PartyService.Providers.ProviderMode ProviderMode { get; private set; }

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            if (System.Configuration.ConfigurationManager.AppSettings["ProviderMode"].ToUpper() == "DBCONTEXT")
                ProviderMode = Providers.ProviderMode.DbContent;
            else
                ProviderMode = Providers.ProviderMode.TestContent;
		}        
	}
}
