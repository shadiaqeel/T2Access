﻿using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

namespace T2Access.Web.Attributes
{
    public class LogActionFilter : ActionFilterAttribute

    {
        private readonly Stopwatch sw = new Stopwatch();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            sw.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            sw.Stop();

            Log("OnResultExecuted", filterContext.RouteData, sw.ElapsedMilliseconds.ToString());
        }


        private void Log(string methodName, RouteData routeData, string Data)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = string.Format(" Logging  MVC !!!!!   controller:{0} action:{1}  Elapsed Milliseconds :{2} ", controllerName, actionName, Data);

            Debug.WriteLine(message);
        }

    }

}