using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

using T2Access.Models;
using T2Access.Services.HttpClientService;
using T2Access.Web.Attributes;

namespace T2Access.Web.Controllers
{

  [CustomAuthorize]

    public class GateController : Controller
    {


        IHttpClientService httpService = new HttpClientService(new Uri(Variables.ServerBaseAddress + "gate/"));


        // GET: Gate
        public ActionResult Index()
        {
            return View();
        }





        #region Admin

        #region Accounts Managment

        #region Gate Managment


        public async Task<ActionResult> GateManagment()
        {

            var response = await httpService.GetAsync("GetListWithFilter/", token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)
            {
                var Gates = await response.Content.ReadAsAsync<List<GateModel>>();

                return View(Gates);
            }
            else
                return View();
        }





        #region Create Gate Account
        public ActionResult Create()
        {

            return PartialView("_Create");


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GateSignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create");
            }

            var response = await httpService.PostAsync("Signup/", model, token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)

                // return RedirectToAction("GateManagment");
                return  Json(new { success = true });
            else
            {


                var error = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = error;

               // ModelState.AddModelError("UserName", error);


                return PartialView("_Create", model);


            }

        }
        #endregion



        #region Delete
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
            return RedirectToAction("GateManagment");




        }
        #endregion


        #region Edit

        public ActionResult Edit()
        {

            return PartialView("_Edit");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( GateModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit");
            }

            var response = await httpService.PutAsync($"Edit?id={model.Id}", model, token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)

                // return RedirectToAction("GateManagment");
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

            var response = await httpService.PutAsync($"ResetPassword?id={model.Id}", model, token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)

                return Json(new { success = true });
            else
            {


                var error = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = error;



                return PartialView("_Edit", model);


            }

        }

        #endregion

        #endregion
        #endregion
        #endregion


    }
}