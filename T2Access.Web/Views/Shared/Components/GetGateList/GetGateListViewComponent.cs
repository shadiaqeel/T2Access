using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace T2Access.Web.Views.Shared.Components.GetGateList
{
    public class GetGateListViewComponent : ViewComponent
    {
        public GetGateListViewComponent(){}
        public IViewComponentResult Invoke()
        {
            return View("Default");
        }

    }
}
