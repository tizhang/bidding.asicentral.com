using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ASI.Services.Statistics.Data;
using WebApiTemplate.Controllers;
using WebApiTemplate.Models;

namespace WebApiTemplate.Filters
{
    public class StatsActionFilter : ActionFilterAttribute
    {
        private ExecutionTimeRecord _record;

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.ActionContext.ControllerContext.Controller is HealthController || actionExecutedContext.ActionContext.ControllerContext.Controller is MetricsController) return;

            if (_record == null) return;

            _record.Stop();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ControllerContext.Controller is HealthController || actionContext.ControllerContext.Controller is MetricsController) return;

            var collector = Collector.Current;
            if (collector == null) return;
            _record = collector.Append<ExecutionTimeRecord>();
            _record.Method = actionContext.Request.RequestUri.AbsolutePath;
            _record.Start();
        }
    }
}