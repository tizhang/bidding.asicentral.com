using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using ExceptionHandler = ASI.Services.Http.Exceptions.ExceptionHandler;

namespace WebApiTemplate
{
    public static class ExceptionsConfig
    {
        public static void Register(HttpConfiguration config)
        {
            bool.TryParse(ConfigurationManager.AppSettings["Errors:IncludeStackTrace"], out bool includeStackTrace);

            var logger = new Services.ExceptionLogger();

            var handler = new ExceptionHandler(logger, includeStackTrace);

            handler.Register<ApplicationException>(HttpStatusCode.BadRequest)
                .Register<FileNotFoundException>(HttpStatusCode.NotFound);

            //var handler = new ExceptionHandler(includeStackTrace);
            config.Services.Replace(typeof(IExceptionHandler), handler);
            //config.Filters.Add(new UnhandledExceptionFilterAttribute(handler));
        }
    }
}