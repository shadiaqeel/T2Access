using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

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


        public async Task<ActionResult> GetAll()
        {


            var response = await httpService.GetAsync("GetListWithFilter/", token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadAsAsync<List<UserViewModel>>();

                if (Request.IsAjaxRequest())
                {
                    //Server Side Parameter
                    int start = Convert.ToInt32(Request["start"]);
                    int length = Convert.ToInt32(Request["length"]);
                    string searchValue = Request["search[value]"];
                    string sortColumnName = Request[$"columns[{Request["order[0][column]"]}][name]"];
                    string sortDirection = Request["order[0][dir]"];



                    int totalrows = users.Count;

                    //filter
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        users = users.Where(x => x.UserName.ToLower().Contains(searchValue.ToLower())
                                        || x.FirstName.ToLower().Contains(searchValue.ToLower())
                                        || x.LastName.ToLower().Contains(searchValue.ToLower())
                        ).ToList<UserViewModel>();

                    }
                    int totalrowsafterfiltering = users.Count;


                    //sorting 
                    if (!string.IsNullOrEmpty(sortColumnName))
                        users = users.OrderBy($"{sortColumnName} {sortDirection}").ToList<UserViewModel>();

                    //paging
                    users = users.Skip(start).Take(length).ToList<UserViewModel>();



                    return Json(new { view = RenderViewToString(ControllerContext, "GetAll", users, true), draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

                return PartialView(users);
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
        public async Task<ActionResult> Create(UserSignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }



            var response = await httpService.PostAsync(" Signup/", model, token: (string)Session["Token"]);
            var result = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {

                TempData["toastrMessage"] = result.Split('\"')[1];
                return RedirectToAction("UserManagment");
            }
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


        public async Task<ActionResult> Edit(Guid id)
        {

            var response = await httpService.GetAsync($"GetById/?id={id}", token: (string)Session["Token"]);
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadAsAsync<UserViewModel>();
                return View(user);

            }

            return null;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel model)
        {

            model.UserName = null;


            var response = await httpService.PutAsync($"Edit?id={model.Id}", model, token: (string)Session["Token"]);
            var result = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                TempData["toastrMessage"] = result.Split('\"')[1];

                return RedirectToAction("UserManagment");
            }

            else
            {

                var error = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = error;

                // ModelState.AddModelError("UserName", error);

                return View(model);

            }

        }





        #endregion


        public async Task<ActionResult> Delete(Guid id)
        {



            var response = await httpService.DeleteAsync($"Delete?id={id}", (string)Session["Token"]);

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                return Json(new { success = true, message = result.Split('\"') }, JsonRequestBehavior.AllowGet);

            }
            else
            {

                return Json(new { success = false, message = result.Split('\"') }, JsonRequestBehavior.AllowGet);

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
