using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using ExceptionHandler = ASI.Services.Http.Exceptions.ExceptionHandler;

namespace Bidding.Api
{
    public static class ExceptionsConfig
    {
        public static void Register(HttpConfiguration config)
        {
            bool.TryParse(ConfigurationManager.AppSettings["Errors:IncludeStackTrace"], out bool includeStackTrace);

            bool.TryParse(ConfigurationManager.AppSettings["Errors:CaptureBaseException"], out bool captureBaseException);

            var logger = new Services.ExceptionLogger();

            var handler = new ExceptionHandler(logger, includeStackTrace, captureBaseException);

            handler.Register<ApplicationException>(HttpStatusCode.BadRequest)
                .Register<FileNotFoundException>(HttpStatusCode.NotFound);

            //var handler = new ExceptionHandler(includeStackTrace);
            config.Services.Replace(typeof(IExceptionHandler), handler);
            //config.Filters.Add(new UnhandledExceptionFilterAttribute(handler));
        }
    }
}