using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;


using T2Access.Services.HttpClientService;
using T2Access.Web.SPA.VueJs.Models;
using T2Access.Web.SPA.VueJs.Areas.Admin.Models;
using T2Access.Models;
using T2Access.Web.SPA.VueJs.Extensions;

namespace T2Access.Web.SPA.VueJs.Areas.Admin
{
    public class UserController : AdminController
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

        //public async Task<IActionResult> Get(DTParameters param)
        [HttpPost]
        public async Task<IActionResult> LoadData([FromBody]DTUserParameters param)
        {
            //! Server Side 

            using (var response = await _httpService.GetAsync($"GetListWithFilter?UserName={param.Filter?.Username}&FirstName={param.Filter?.Firstname }&LastName={param.Filter?.Lastname}&Status={param.Filter?.Status }&Skip={param.Start}&PageSize={param.Length}", token: HttpContext.Session.GetString("Token")))
            {
                if (response.IsSuccessStatusCode)
                {
                    //var users = await response.Content.ReadAsAsync<ListResponse<UserViewModel>>();
                    //int totalRowsAfterFiltering = users.ResponseList.Count();
                     //return Json(new { data = JsonConvert.SerializeObject(users.ResponseList, new T2Access.Web.SPA.VueJs.DisplayEnumConverter()), draw = Request.Form["draw"], recordsTotal = users.TotalEntities });

                    var users = JsonConvert.DeserializeObject<ListResponse<UserViewModel>>(await response.Content.ReadAsStringAsync());

                    return Ok(new
                    {
                        users = users.ResponseList,
                        recordsTotal = users.TotalEntities
                    });

                }


                return BadRequest(await response.Content.ReadAsStringAsync());



            }
        }

 // ====================================== Create User =============================================
        #region Create

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody]SignUpUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateErrors());
            }

            using (var response = await _httpService.PostAsync(" Signup/", model, token: HttpContext.Session.GetString("Token")))
            {
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Ok(result.Replace("\"", ""));
                }
                else
                {

                    return BadRequest(result.Replace("\"", ""));
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



    





     // ====================================== Edit User =============================================

        #region Edit
        public async Task<IActionResult> GetById (Guid id)
        {
            using (var response = await _httpService.GetAsync($"GetById/?id={id}", token: HttpContext.Session.GetString("Token")))
            {
                var result = JsonConvert.DeserializeObject<UserViewModel>(await response.Content.ReadAsStringAsync());


                if (response.IsSuccessStatusCode)
                {
                    return Ok(result);
                }

                    return BadRequest(result);
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody]UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateErrors());
            }
            model.UserName = null;

            using (var response = await _httpService.PutAsync($"Edit?id={model.Id}", model, token: HttpContext.Session.GetString("Token")))
            {
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Ok(result.Replace("\"", ""));

                }
                else
                {
                    Trace.WriteLine($"  Error Model :    {model}  ");

                    return BadRequest(model);
                    // ViewBag.ErrorMessage = result.Replace("\"", "");

                }
            }
        }
        #endregion
        }
}