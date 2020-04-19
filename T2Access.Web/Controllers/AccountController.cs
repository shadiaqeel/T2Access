using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IAuthService _authService;
        //=======================================================================

        public AccountController(IHttpClientService httpService, IAuthService authService)
        {
            _authService = authService ?? new JWTService();
            this.httpService = httpService;
            this.httpService.BaseUri = new Uri($"{this.httpService.BaseUri}{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}/User/");
        }


        //========================================================================
        [AllowAnonymous]
        public IActionResult Login(string returnUrl=null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null )
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var _returnUrl = returnUrl ?? HttpContext.Session.GetString("returnUrl") ?? null;



            using (var response = await httpService.PostAsync("Login", model))
            {
                if (response.IsSuccessStatusCode)
                {

                    var result = await response.Content.ReadAsStringAsync();


                    string token = result.Replace("\"", "");

                    IEnumerable<Claim> claims = _authService.GetTokenClaims(token);

                    if (claims.Any(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && x.Value == "Admin"))
                    {
                        HttpContext.Session.SetString("Token", token);
                        //Session["Principal"] = authService.GetPrincipal(token);
                        HttpContext.Session.SetString("Username", claims.FirstOrDefault(x => x.Type == "username").Value);
                        HttpContext.Session.SetString("Role", claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Select(x => x.Value).ToString());
                        HttpContext.Session.SetString("FirstName", claims.FirstOrDefault(x => x.Type == "firstName").Value);
                        HttpContext.Session.SetString("LastName", claims.FirstOrDefault(x => x.Type == "lastName").Value);
                        HttpContext.Session.SetString("ConfirmedOperation", false.ToString());
                        HttpContext.Session.SetString("UserImg", "/Assets/Admin/shadi.jpg");

                        if (!string.IsNullOrEmpty(_returnUrl))
                        {
                            return Redirect(_returnUrl);
                        }


                        return RedirectToAction("index", "User");
                    }
                  //  ViewBag.ReturnUrl = _returnUrl;
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

            return RedirectToAction("Login", "Account");
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




                    HttpContext.Session.SetString("Token", token);
                    HttpContext.Session.SetString("Username", _authService.GetTokenClaimValue(token, "username"));
                    HttpContext.Session.SetString("Role", _authService.GetTokenClaimValue(token, "roles"));
                    HttpContext.Session.SetString("ConfirmedOperation", true.ToString());

                    return Json(new { success = true });
                }
            }



            ModelState.AddModelError(string.Empty, Resource.PasswordWrong);
            return PartialView("_ReLogin", model);



        }





    }
}