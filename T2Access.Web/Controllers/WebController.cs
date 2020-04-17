﻿using System.Diagnostics;
using System.IO;

using Microsoft.AspNetCore.Mvc;
using T2Access.Web.Models;

namespace T2Access.Web.Controllers
{
    //[LogActionFilter]
    //[Localisation]
    public class WebController : Controller
    {

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}