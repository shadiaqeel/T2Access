using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


using T2Access.Web.SPA.VueJs.Models;

namespace T2Access.Web.SPA.VueJs.Areas.Admin
{
    //[LogActionFilter]
    //[Localisation]
    [Authorize(Roles ="Admin")]

    public class AdminController : Controller
    {

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}