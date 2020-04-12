using System;
using System.Net.Http;
using System.Threading;
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
        private readonly IHttpClientService httpService = new HttpClientService(new Uri(Variables.ServerBaseAddress + $"{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}/user/"));




        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserManagment()
        {
            if (TempData["successToastrMessage"] != null)
            {
                ViewBag.successToastrMessage = TempData["successToastrMessage"] as string;
            }

            if (TempData["errorToastrMessage"] != null)
            {
                ViewBag.errorToastrMessage = TempData["errorToastrMessage"] as string;
            }

            return View();
        }

        public async Task<ActionResult> LoadData()
        {
            //! Server Side Parameter
            var start = Convert.ToInt32(Request["start"]);
            var length = Convert.ToInt32(Request["length"]);
            var sortColumnName = Request[$"columns[{Request["order[0][column]"]}][name]"];
            var sortDirection = Request["order[0][dir]"];
            ////var searchValue = Request["search[value]"];

            //! find search columns info
            var userName = Request["columns[0][search][value]"];
            var firstName = Request["columns[1][search][value]"];
            var lastName = Request["columns[2][search][value]"];
            var status = Request["columns[3][search][value]"];

            var order = $"{sortColumnName} {sortDirection}";

            using (var response = await httpService.GetAsync($"GetListWithFilter?UserName={userName}&FirstName={firstName}&LastName={lastName}&Status={status}&Skip={start}&PageSize={length}&Order={order}", token: (string)Session["Token"]))
            {
                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadAsAsync<ListResponse<UserViewModel>>();

                    if (Request.IsAjaxRequest())
                    {
                        ////int totalRowsAfterFiltering = users.ResponseList.Count();

                        return Json(new { data = JsonConvert.SerializeObject(users.ResponseList, new T2Access.Web.Helper.DisplayEnumConverter()), draw = Request["draw"], recordsTotal = users.TotalEntities }, JsonRequestBehavior.AllowGet);
                    }

                    return PartialView(users);
                }

                var error = await response.Content.ReadAsStringAsync();

                ViewBag.errorToastrMessage = error;
            }
            return PartialView();
        }



        // ====================================== Create User =============================================
        #region Create
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

            using (var response = await httpService.PostAsync(" Signup/", model, token: (string)Session["Token"]))
            {
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["successToastrMessage"] = result.Replace("\"", "");
                    return RedirectToAction("UserManagment");
                }
                else
                {
                    var error = result.Replace("\"", "").Split(',');
                    ViewBag.errorToastrMessage = error[0];
                    ViewBag.ErrorMessage = error[1] ?? null;

                    /// ModelState.AddModelError("UserName", result.Replace("\"", ""));

                    return View(model);
                }
            }
        }
        #endregion

        // ====================================== Edit User =============================================

        #region Edit
        public async Task<ActionResult> Edit(Guid id)
        {
            using (var response = await httpService.GetAsync($"GetById/?id={id}", token: (string)Session["Token"]))
            {
                var result = await response.Content.ReadAsAsync<UserViewModel>();

                if (response.IsSuccessStatusCode)
                {
                    return View(result);
                }

                ViewBag.errorToastrMessage = result;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel model)
        {
            model.UserName = null;

            using (var response = await httpService.PutAsync($"Edit?id={model.Id}", model, token: (string)Session["Token"]))
            {
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["successToastrMessage"] = result.Replace("\"", "");

                    return RedirectToAction("UserManagment");
                }
                else
                {
                    ViewBag.ErrorMessage = result.Replace("\"", "");
                    ViewBag.errorToastrMessage = result.Replace("\"", "");

                    return View(model);
                }
            }
        }
        #endregion

        // ====================================== Delete User =============================================

        public async Task<ActionResult> Delete(Guid id)
        {
            using (var response = await httpService.DeleteAsync($"Delete?id={id}", (string)Session["Token"]))
            {
                var result = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode
                    ? Json(new { success = true, message = result.Replace("\"", "") }, JsonRequestBehavior.AllowGet)
                    : Json(new { success = false, message = result.Replace("\"", "") }, JsonRequestBehavior.AllowGet);
            }
        }

        // ====================================== Reset User Password =============================================

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

            using (var response = await httpService.PutAsync($"ResetPassword?id={model.Id}", model, token: (string)Session["Token"]))
            {
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = result;
                    ViewBag.errorToastrMessage = result;

                    return PartialView("_ResetPassword", model);
                }
            }

        }
        #endregion


    }
}