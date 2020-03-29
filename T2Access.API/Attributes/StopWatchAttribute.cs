using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace T2Access.API.Attributes
{
    public class StopWatchAttribute : Attribute, IActionFilter
    {
        public bool AllowMultiple { get { return true; } }

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            var res = continuation();
            res.Wait();

            sw.Stop();


            Trace.WriteLine(string.Format(" Logging  API !!!!! Action Method : {0} | Elapsed Milliseconds: {1} ", actionContext.ActionDescriptor.ActionName, sw.ElapsedMilliseconds));
           // Trace.WriteLine(string.Format("Action Method : {0} | Elapsed Ticks: {1} ", actionContext.ActionDescriptor.ActionName, sw.ElapsedTicks), "Logging ! ");


            return res;

        }
    }

}