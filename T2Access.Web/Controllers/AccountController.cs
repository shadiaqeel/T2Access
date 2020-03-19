﻿using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using T2Access.Models;
using T2Access.Security.Tokenization.Services;
using T2Access.Services.HttpClientService;
using T2Access.Web.Helper;
using T2Access.Web.Models;

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
                Session["ConfirmedOperation"] = false;
                Session["UserImg"] = "/Assets/User/shadi.jpg";
                Session["Culture"] = "ar";

                return RedirectToAction("index", "User");
            }



            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View();



        }


        //[AllowAnonymous]
        //public ActionResult Register()
        //{
        //    return View();
        //}


        public ActionResult LogOut()
        {
            Session.Clear();

            return RedirectToAction("index", "Home");
        }




        public ActionResult ReLogin()
        {

            return PartialView("_ReLogin");
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReLogin(LoginModel model )
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ReLogin", model);

            }


            var response = await httpService.PostAsync("user/Login", model);



            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();
                token = token.Replace("\"", "");

                IAuthService authService = AuthrizationFactory.GetAuthrization();



                Session["Token"] = token;
                Session["Username"] = authService.GetTokenClaimValue(token, "Username");
                Session["Role"] = authService.GetTokenClaimValue(token, "Role");
                Session["ConfirmedOperation"] = true;

                return Json(new { success = true });
            }



            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return PartialView("_ReLogin",model);



        }




    }
}