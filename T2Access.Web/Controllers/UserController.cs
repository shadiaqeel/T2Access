﻿using System;
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

        IHttpClientService httpService = new HttpClientService(new Uri(Variables.ServerBaseAddress + "user/"));


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


            var response = await httpService.GetAsync("GetListWithFilter/", token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)
            {
                var Users = await response.Content.ReadAsAsync<List<UserModel>>();

                return View(Users);
            }




            return View();
        }




        public async Task<ActionResult> Delete(Guid id)
        {

            var response = await httpService.DeleteAsync($"Delete?id={id}", (string)Session["Token"]);

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                ViewBag.StateMessage = result;

            }
            else
            {

                ViewBag.ErrorMessage = result;

            }
            return RedirectToAction("UserManagment");




        }


        #region Create User
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

            var response = await httpService.PostAsync("    Signup/", model, token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)

                return RedirectToAction("UserManagment");
            else
            {


                var error = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = error;

                ModelState.AddModelError("UserName", error);


                return View(model);


            }

        }
        #endregion

        #region Edit

        public ActionResult Edit()
        {

            return PartialView("_Edit");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit");
            }

            var response = await httpService.PutAsync($"Edit?id={model.Id}", model, token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)

                return Json(new { success = true });
            else
            {


                var error = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = error;

                // ModelState.AddModelError("UserName", error);


                return PartialView("_Edit", model);


            }

        }





        #endregion

        #region Reset Password

        public ActionResult ResetPassword()
        {

            return PartialView("_ResetPassword");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ResetPassword");
            }


            if (!(bool)Session["ConfirmedOperation"])
            {

                return Json(new { confirm = true });
            }
            Session["ConfirmedOperation"] = false;



            var response = await httpService.PutAsync($"ResetPassword?id={model.Id}", model, token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)

                return Json(new { success = true });
            else
            {


                var error = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = error;



                return PartialView("_ResetPassword", model);


            }

        }

        #endregion



        #endregion



        #endregion


        #endregion





    }
}