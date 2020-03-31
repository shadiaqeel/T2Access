using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using T2Access.Models;
using T2Access.Services.HttpClientService;
using T2Access.Web.Attributes;

namespace T2Access.Web.Controllers
{

    [CustomAuthorize]
    public class UserController : WebController
    {
        readonly IHttpClientService httpService = new HttpClientService(new Uri(Variables.ServerBaseAddress + "user/"));



        // GET: User
        public ActionResult Index()
        {
            return View();
        }





        #region Admin

        #region Accounts Managment

        #region User Managment






        public ActionResult UserManagment()
        {
            if (TempData["toastrMessage"] != null)
                ViewBag.toastrMessage = TempData["toastrMessage"] as string;

            return View();
        }


        public async Task<ActionResult> LoadData()
        {

            //Server Side Parameter
            var draw = Convert.ToInt32(Request["draw"]);
            var start = Convert.ToInt32(Request["start"]);
            var length = Convert.ToInt32(Request["length"]);
            var sortColumnName = Request[$"columns[{Request["order[0][column]"]}][name]"];
            var sortDirection = Request["order[0][dir]"];
            //var searchValue = Request["search[value]"];




            //find search columns info
            var userName = Request["columns[0][search][value]"];
            var firstName = Request["columns[1][search][value]"];
            var lastName = Request["columns[2][search][value]"];
            var status = Request["columns[3][search][value]"];



            var order= $"{sortColumnName} {sortDirection}";

            var response = await httpService.GetAsync($"GetListWithFilter?UserName={userName}&FirstName={firstName}&LastName={lastName}&Status={status}&Skip={start}&PageSize={length}&Order={order}", token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<ServiceResponse<ListResponse<UserViewModel>>>();

                if (result.Success)
                {

                    var users = result.Data;

                if (Request.IsAjaxRequest())
                    {

                        int totalrowsafterfiltering = users.ResponseList.Count;

                        return Json(new { data = JsonConvert.SerializeObject(users.ResponseList, new T2Access.Web.Helper.DisplayEnumConverter()), draw = Request["draw"], recordsTotal = users.totalEntities }, JsonRequestBehavior.AllowGet);
                    }

                    return PartialView(users);
                }


                ViewBag.toastrMessage = result.Message;
                return PartialView(); 
            }
            return PartialView();
        }



        #region Create User
        public ActionResult Create()
        {

            return View();


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SignUpUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }



            var response = await httpService.PostAsync(" Signup/", model, token: (string)Session["Token"]);
            var result = await response.Content.ReadAsAsync<ServiceResponse<string>>();


            if (response.IsSuccessStatusCode)
            {

                TempData["toastrMessage"] = result.Data.Split('\"')[1];
                return RedirectToAction("UserManagment");
            }
            else
            {


                //var error = await response.Content.ReadAsStringAsync();
                //ViewBag.ErrorMessage = error;

                ViewBag.toastrMessage = result.Message;

                ModelState.AddModelError("UserName", result.Message);


                return View(model);


            }

        }
        #endregion


        #region Edit


        public async Task<ActionResult> Edit(Guid id)
        {

            var response = await httpService.GetAsync($"GetById/?id={id}", token: (string)Session["Token"]);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<ServiceResponse<UserViewModel>>();
                if(result.Success)
                    return View(result.Data);

                ViewBag.toastrMessage = result.Message;
                return View();

            }

            return null;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel model)
        {

            model.UserName = null;


            var response = await httpService.PutAsync($"Edit?id={model.Id}", model, token: (string)Session["Token"]);
            var result = await response.Content.ReadAsAsync<ServiceResponse<string>>();


            if (response.IsSuccessStatusCode)
            {
                TempData["toastrMessage"] = result.Data.Split('\"')[1];

                return RedirectToAction("UserManagment");
            }
            else
            {

                ViewBag.ErrorMessage = result.Message;

                // ModelState.AddModelError("UserName", error);

                return View(model);

            }

        }





        #endregion


        public async Task<ActionResult> Delete(Guid id)
        {



            var response = await httpService.DeleteAsync($"Delete?id={id}", (string)Session["Token"]);

            var result = await response.Content.ReadAsAsync<ServiceResponse<string>>();

            if (response.IsSuccessStatusCode)
            {

                return Json(new { success = true, message = result.Data.Split('\"') }, JsonRequestBehavior.AllowGet);

            }
            else
            {

                return Json(new { success = false, message = result.Data.Split('\"') }, JsonRequestBehavior.AllowGet);

            }




        }




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


                var result = await response.Content.ReadAsAsync<ServiceResponse<string>>();
                ViewBag.ErrorMessage = result.Message;


                return PartialView("_ResetPassword", model);


            }

        }

        #endregion



        #endregion



        #endregion


        #endregion




        


       
    }
}
