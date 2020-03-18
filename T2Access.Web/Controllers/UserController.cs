using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

using T2Access.Models;
using T2Access.Services.HttpClientService;
using T2Access.Web.Attributes;

namespace T2Access.Web.Controllers
{


    [CustomAuthorize]
    public class UserController : Controller
    {

        IHttpClientService httpService = new HttpClientService(new Uri(Variables.ServerBaseAddress));


        // GET: User
        public ActionResult Index()
        {
            return View();
        }





        #region Admin

        #region Accounts Managment

        #region User Managment


        public async Task<ActionResult> UserManagment()
        {


            var response = await httpService.GetAsync("user/GetListWithFilter/", token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)
            {
                var Users = await response.Content.ReadAsAsync<List<UserModel>>();

                return View(Users);
            }




            return View();
        }

        public async Task< ActionResult> Delete(Guid id)
        {

            var response = await httpService.DeleteAsync($"user/Delete?id={id}",(string)Session["Token"]);

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                ViewBag.StateMessage = result;

            }
            else {

                ViewBag.ErrorMessage = result;

            }
            return RedirectToAction("UserManagment");




        }


        public ActionResult Create()
        {

            return View();


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserSignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }



            var response = await httpService.PostAsync("user/Signup/", model, token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)

                return RedirectToAction("UserManagment");
            else
            {


                var error = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = error;

                ModelState.AddModelError("UserName",error);


                return View(model);
            
            
            }

        }



        #endregion



        #endregion


        #endregion





    }
}