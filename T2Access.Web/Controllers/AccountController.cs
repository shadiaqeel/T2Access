using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using T2Access.Security.Tokenization.Services;
using T2Access.Services.HttpClientService;
using T2Access.Web.Helper;
using T2Access.Web.Models;
using T2Access.Web.Filters;
using System.Collections;
using T2Access.Web.Attributes;

namespace T2Access.Web.Controllers
{
    public class AccountController : Controller
    {



        IHttpClientService httpService = new HttpClientService(new Uri(Variables.ServerBaseAddress));


        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var response = await httpService.PostAsync("user/Login", model);



            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();
                token = token.Replace("\"", "");

                IAuthService authService = AuthrizationFactory.GetAuthrization();

               
               
                Session["Token"] = token;
                //Session["Principal"] = authService.GetPrincipal(token);
                Session["Username"] = authService.GetTokenClaimValue(token, "Username");
                Session["Role"] = authService.GetTokenClaimValue(token, "Role");
                Session["FirstName"] = authService.GetTokenClaimValue(token, "FirstName");
                Session["LastName"] = authService.GetTokenClaimValue(token, "LastName");


                return RedirectToAction("index", "Home");
            }



            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View();



        }





        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }


        public ActionResult LogOff()
        {
            Session.Clear();

            return RedirectToAction("index", "Home");
        }





    }
}