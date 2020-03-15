using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using T2Access.Web.Models;
using T2Access.Security.Tokenization.Services;
using  T2Access.Web.Helper;
using System.Security.Principal;
using System.Threading;
using T2Access.Services.HttpClientService;

namespace T2Access.Web.Controllers
{
    public class AccountController : Controller
    {



        IHttpClientService httpService = new HttpClientService(new Uri (Variables.ServerBaseAddress));


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


           // var response =  await httpService.PostAsync("user/Login", model);
           

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Variables.ServerBaseAddress);


                // var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var postTask =  client.PostAsync("user/Login", content);
                postTask.Wait();

                
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    string token = await result.Content.ReadAsStringAsync();
                    token = token.Replace("\"", "");
                    
                    IAuthService authService = AuthrizationFactory.GetAuthrization();

                   IPrincipal principal =  authService.GetPrincipal(token);


                    Thread.CurrentPrincipal = principal;
                    System.Web.HttpContextBase httpContext = HttpContext;
                    httpContext.User = principal;

                    return RedirectToAction("index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View();


            }
        }





        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }






    }
}