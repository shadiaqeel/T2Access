using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;


using T2Access.Services.HttpClientService;
using T2Access.Web.SPA.VueJs.Models;
using T2Access.Models;

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
        public async Task<IActionResult> Get(string start, string length)
        {
            //! Server Side 

            //using (var response = await _httpService.GetAsync($"GetListWithFilter?UserName={param.Columns[0].Search.Value}&FirstName={param.Columns[1].Search.Value }&LastName={param.Columns[2].Search.Value}&Status={param.Columns[3].Search.Value }&Skip={param.Start}&PageSize={param.Length}&Order={param.SortOrder}", token: HttpContext.Session.GetString("Token")))
            using (var response = await _httpService.GetAsync($"GetListWithFilter?Skip={start}&PageSize={length}", token: HttpContext.Session.GetString("Token")))
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

                // ====================================== Delete User =============================================

        public async Task<IActionResult> Delete(Guid id)
        {
            // using (var response = await _httpService.DeleteAsync($"Delete?id={id}", HttpContext.Session.GetString("Token")))
            // {
            //     var result = await response.Content.ReadAsStringAsync();

            //     return response.IsSuccessStatusCode
            //         ? Json(new { success = true, message = result.Replace("\"", "") })
            //         : Json(new { success = false, message = result.Replace("\"", "") });
            // }
            return BadRequest("shadi");
        }



    }
}