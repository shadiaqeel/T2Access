using System.Web.Mvc;

namespace T2Access.Web.Controllers
{
    // [CustomAuthorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty((string)Session["Token"]))
                return RedirectToAction("Login", "Account");

            return RedirectToAction("Index", "Admin");


        }



    }
}