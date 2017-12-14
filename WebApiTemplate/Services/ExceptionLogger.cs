using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using ASI.Services.Http;
using ASI.Services.Http.Exceptions;
using ASI.Services.Http.Logging;
using ASI.Services.Http.Security;
using log4net;
using ExceptionHandler = System.Reflection.Emit.ExceptionHandler;

namespace WebApiTemplate.Services
{
    public class ExceptionLogger : ASI.Services.Http.Exceptions.IExceptionLogger
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ExceptionLogger));

        public void Capture(Exception exception)
        {
            if (exception == null) return;
            _logger.Error(exception.GetBaseException());
        }

        public string Translate(Exception exception)
        {
            return exception?.GetBaseException().Message;
        }
    }
}