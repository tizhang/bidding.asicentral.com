using log4net.Config;
using System.IO;
using System.Web;
using System.Web.Http;
using ASI.Services.Messaging;
using WebApi.StructureMap;
using Bidding.Bol;

namespace Bidding.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // Initialize log4net 
            InitializeLogging();

            // Initialize DI
            GlobalConfiguration.Configuration.UseStructureMap(ServiceConfig.RegisterServices);
            
            // Web API configuration
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Enable (unhandled) exception handling
            GlobalConfiguration.Configure(ExceptionsConfig.Register);

            // Enable stats and health checks
            GlobalConfiguration.Configure(StatsConfig.Register);

            BiddingManager.Initialize();

            //ScheduleConfig.Setup();
            var i = BiddingJobScheduler.Instance;
        }

        protected void Application_End()
        {
            //ScheduleConfig.Cleanup();
            // Cleanup ESB connections
            PonyEsb.CloseAll();
        }

        private static void InitializeLogging()
        {
            var log4NetConfig = System.Web.Hosting.HostingEnvironment.MapPath("~/log4net.config");
            if (log4NetConfig != null && File.Exists(log4NetConfig))
            {
                XmlConfigurator.Configure(new FileInfo(log4NetConfig));
            }
        }
    }
}
