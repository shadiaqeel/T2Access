using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using T2Access.Models;
using T2Access.Security.Tokenization.Services;
using T2Access.Services.HttpClientService;
using T2Access.Web.Resources;

namespace T2Access.Web.Controllers
{
    public class AccountController : WebController
    {
        private readonly IHttpClientService httpService;
        //=======================================================================

        public AccountController(IHttpClientService httpService)
        {
            this.httpService = httpService;
            this.httpService.BaseUri = new Uri( $"{this.httpService.BaseUri}{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}/User/" );
        }


        //========================================================================
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            using (var response = await httpService.PostAsync("Login", model))
            {
                if (response.IsSuccessStatusCode)
                {

                    var result = await response.Content.ReadAsStringAsync();


                    string token = result.Replace("\"", "");


                    IAuthService authService = new JWTService();

                    if (authService.GetTokenClaimValue(token, "roles").Contains("Admin"))
                    {
                        HttpContext.Session.SetString("Token", token);
                        //Session["Principal"] = authService.GetPrincipal(token);
                       HttpContext.Session.SetString("Username",authService.GetTokenClaimValue(token, "username"));
                       HttpContext.Session.SetString("Role",authService.GetTokenClaimValue(token, "roles"));
                       HttpContext.Session.SetString("FirstName" , authService.GetTokenClaimValue(token, "firstName"));
                       HttpContext.Session.SetString("LastName" , authService.GetTokenClaimValue(token, "lastName"));
                       HttpContext.Session.SetString("ConfirmedOperation" , false.ToString());
                       HttpContext.Session.SetString("UserImg" , "/Assets/Admin/shadi.jpg");

                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        return RedirectToAction("index", "User");
                    }
                    ViewBag.ReturnUrl = returnUrl;
                    ModelState.AddModelError(string.Empty, Resource.NotAuthorized);
                    return View();
                }
            }


            ViewBag.ReturnUrl = returnUrl;
            ModelState.AddModelError(string.Empty, Resource.LoginFailed);
            return View();



        }


        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("index", "Home");
        }


        // ====================================== Relogin =============================================

        public IActionResult ReLogin()
        {

            return PartialView("_ReLogin");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReLogin(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ReLogin", model);

            }


            using (var response = await httpService.PostAsync("Login", model))
            {
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();

                    IAuthService authService = new JWTService();



                   HttpContext.Session.SetString("Token" ,token);
                   HttpContext.Session.SetString("Username" , authService.GetTokenClaimValue(token, "username"));
                   HttpContext.Session.SetString("Role", authService.GetTokenClaimValue(token, "roles"));
                   HttpContext.Session.SetString("ConfirmedOperation" , true.ToString());

                    return Json(new { success = true });
                }
            }



            ModelState.AddModelError(string.Empty, Resource.PasswordWrong);
            return PartialView("_ReLogin", model);



        }





    }
}