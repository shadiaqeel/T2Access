using Microsoft.AspNetCore.Mvc;
using T2Access.Models;

namespace T2Access.Web.Views.Shared.Components.GetGateList
{
    public class GetGateListViewComponent : ViewComponent
    {
        public GetGateListViewComponent() { }
        public IViewComponentResult Invoke()
        {
            return View("Default" , new GateViewModel());
        }

    }
}
