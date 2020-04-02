﻿using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using T2Access.Models;
using T2Access.Security.Tokenization.Services;
using T2Access.Services.HttpClientService;
using T2Access.Web.Helper;
using T2Access.Web.Models;
using T2Access.Web.Resources;

namespace T2Access.Web.Controllers
{
    public class AccountController : WebController
    {



        IHttpClientService httpService = new HttpClientService(new Uri(Variables.ServerBaseAddress + $"{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}/User/"));


        // GET: AccountC:\Users\dell\Source\Repos\T2Access\T2Access.Web\Controllers\AccountController.cs
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


            var response = await httpService.PostAsync("Login", model);


            if (response.IsSuccessStatusCode)
            {
                 
                var result = await response.Content.ReadAsStringAsync();


                string token = result.Replace("\"", "");


                IAuthService authService = AuthrizationFactory.GetAuthrization();

                if (authService.GetTokenClaimValue(token, "Role").Contains("Admin"))
                {
                    Session["Token"] = token;
                    //Session["Principal"] = authService.GetPrincipal(token);
                    Session["Username"] = authService.GetTokenClaimValue(token, "Username");
                    Session["Role"] = authService.GetTokenClaimValue(token, "Role");
                    Session["FirstName"] = authService.GetTokenClaimValue(token, "FirstName");
                    Session["LastName"] = authService.GetTokenClaimValue(token, "LastName");
                    Session["ConfirmedOperation"] = false;
                    Session["UserImg"] = "/Assets/Admin/shadi.jpg";
                    Session["Culture"] = "ar";

                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("index", "User");
                }
                ViewBag.ReturnUrl = returnUrl;
                ModelState.AddModelError(string.Empty, Resource.NotAuthorized);
                return View();
            }


            ViewBag.ReturnUrl = returnUrl;
            ModelState.AddModelError(string.Empty, Resource.LoginFailed);
            return View();



        }





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


            var response = await httpService.PostAsync("Login", model);



            if (response.IsSuccessStatusCode)
            {
                var result= await response.Content.ReadAsStringAsync();
                string token = result.Replace("\"", "");

                IAuthService authService = AuthrizationFactory.GetAuthrization();



                Session["Token"] = token;
                Session["Username"] = authService.GetTokenClaimValue(token, "Username");
                Session["Role"] = authService.GetTokenClaimValue(token, "Role");
                Session["ConfirmedOperation"] = true;

                return Json(new { success = true });
            }



            ModelState.AddModelError(string.Empty, Resource.PasswordWrong);
            return PartialView("_ReLogin",model);



        }





        //// Regex to find only the language code part of the URL - language (aa) or locale (aa-AA) syntax
        //static readonly Regex removeLanguage = new Regex(@"/[a-z]{2}/|/[a-z]{2}-[a-zA-Z]{2}/", RegexOptions.Compiled);

        //[AllowAnonymous]
        //public ActionResult ChangeLanguage(string id)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        // Decode the return URL and remove any language selector from it
        //        id = Server.UrlDecode(id);
        //        id = removeLanguage.Replace(id, @"/");
        //        return Redirect(id);
        //    }
        //    return Redirect(@"/");
        //}


    }
}