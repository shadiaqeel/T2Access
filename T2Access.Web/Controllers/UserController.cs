using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using T2Access.Models;
using T2Access.Services.HttpClientService;
using T2Access.Web.Models;

    namespace T2Access.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UserController : WebController
    {
        private readonly IHttpClientService _httpService;
        private readonly ILogger<UserController> _logger;

        //=======================================================================

        public UserController(IHttpClientService httpService, ILogger<UserController> logger)
        {
            _logger = logger;
            _httpService = httpService;
            _httpService.BaseUri = new Uri($"{_httpService.BaseUri}{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}/user/");
        }


        //========================================================================

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserManagment()
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadData(DTParameters param)
        {
            //! Server Side 

            using (var response = await _httpService.GetAsync($"GetListWithFilter?UserName={param.Columns[0].Search.Value}&FirstName={param.Columns[1].Search.Value }&LastName={param.Columns[2].Search.Value}&Status={param.Columns[3].Search.Value }&Skip={param.Start}&PageSize={param.Length}&Order={param.SortOrder}", token: HttpContext.Session.GetString("Token")))
            {
                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadAsAsync<ListResponse<UserViewModel>>();
                    //var users = JsonConvert.DeserializeObject<ListResponse<UserViewModel>>(await response.Content.ReadAsStringAsync());


                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        ////int totalRowsAfterFiltering = users.ResponseList.Count();

                        return Json(new { data = JsonConvert.SerializeObject(users.ResponseList, new T2Access.Web.Helper.DisplayEnumConverter()), draw = Request.Form["draw"], recordsTotal = users.TotalEntities });
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SignUpUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            using (var response = await _httpService.PostAsync(" Signup/", model, token: HttpContext.Session.GetString("Token")))
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
        public async Task<IActionResult> Edit(Guid id)
        {
            using (var response = await _httpService.GetAsync($"GetById/?id={id}", token: HttpContext.Session.GetString("Token")))
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
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            model.UserName = null;

            using (var response = await _httpService.PutAsync($"Edit?id={model.Id}", model, token: HttpContext.Session.GetString("Token")))
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

        public async Task<IActionResult> Delete(Guid id)
        {
            using (var response = await _httpService.DeleteAsync($"Delete?id={id}", HttpContext.Session.GetString("Token")))
            {
                var result = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode
                    ? Json(new { success = true, message = result.Replace("\"", "") })
                    : Json(new { success = false, message = result.Replace("\"", "") });
            }
        }

        // ====================================== Reset User Password =============================================

        #region Reset Password 
        public IActionResult ResetPassword()
        {
            return PartialView("_ResetPassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ResetPassword");
            }

            if (!HttpContext.Session.GetString("ConfirmedOperation").Equals("true"))
            {
                return Json(new { confirm = true });
            }
            HttpContext.Session.SetString("ConfirmedOperation", "false");

            using (var response = await _httpService.PutAsync($"ResetPassword?id={model.Id}", model, token: HttpContext.Session.GetString("Token")))
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