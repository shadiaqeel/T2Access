using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;



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
using T2Access.Models.Dtos;

namespace T2Access.Web.SPA.VueJs.Areas.Admin
{
    public class GateController : AdminController
    {
        private readonly IHttpClientService _httpService;
        private readonly ILogger<GateController> _logger;

        //=======================================================================

        public GateController(IHttpClientService httpService, ILogger<GateController> logger)
        {
            _logger = logger;
            _httpService = httpService;
            _httpService.BaseUri = new Uri($"{_httpService.BaseUri}{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}/gate/");
        }


        // ====================================== Get List Gate =============================================


        #region Get List


        [HttpPost]
        public async Task<IActionResult> LoadData([FromBody]DTGateParameters param)
        {
            // TODO : ServerSide in GetFiltered method
            using (var response = await _httpService.GetAsync($"GetListWithFilter?UserName={param.Filter?.Username}&NameAr={param.Filter?.Namear}&NameEn={param.Filter?.Nameen}&Status={param.Filter?.Status} &PageSize={param.Length}&Skip={param.Start}", token: HttpContext.Session.GetString("Token")))  // &Order={param.SortOrder}
            {

                var filterdGates = JsonConvert.DeserializeObject<ListResponse<GateViewModel>>(await response.Content.ReadAsStringAsync());



                var totalrows = filterdGates.ResponseList.Count();

                // if (!string.IsNullOrEmpty(param.Search.Value))
                // {
                //     filterdGates.ResponseList = filterdGates.ResponseList.Where(x => x.UserName.ToLower().Contains(param.Search.Value.ToLower())
                //                     || x.NameAr.ToLower().Contains(param.Search.Value.ToLower())
                //                     || x.NameEn.ToLower().Contains(param.Search.Value.ToLower())
                //     ).ToList<GateViewModel>();

                // }
                var totalrowsafterfiltering = filterdGates.ResponseList.Count();



                return Ok(new { list = filterdGates.ResponseList, recordsTotal = filterdGates.TotalEntities, recordsFiltered = filterdGates.TotalEntities });

            }

        }


        [HttpPost]
        public async Task<IActionResult> GetCheckedByUserId(Guid id, [FromBody]DTGateParameters param)
        {
            //Server Side Parameter


            using (var response = await _httpService.GetAsync($"GetCheckedListByUserId?Id={id}&Skip={param.Start}&PageSize={param.Length}", token: HttpContext.Session.GetString("Token"))) //&SearchValue={param.Search.Value}
            {
                if (response.IsSuccessStatusCode)
                {
                    var CheckedGates = JsonConvert.DeserializeObject<ListResponse<CheckedGateDto>>(await response.Content.ReadAsStringAsync());

                    return Ok(new { list = CheckedGates.ResponseList, recordsTotal = CheckedGates.TotalEntities, recordsFiltered = CheckedGates.TotalEntities });



                    //return string.IsNullOrEmpty(param.Search.Value)
                    //    ? Json(new { data = CheckedGates.ResponseList, draw = Request.Form["draw"], recordsTotal = CheckedGates.TotalEntities })
                    //    : Json(new { data = CheckedGates.ResponseList, draw = Request.Form["draw"], recordsTotal = CheckedGates.ResponseList.Count() });
                }
            }


            return null;
        }
#endregion


        // ====================================== Create Gate =============================================

        #region Create


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody]SignUpGateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateErrors());
            }

            using (var response = await _httpService.PostAsync("Signup/", model, token: HttpContext.Session.GetString("Token")))
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

        // ====================================== Edit Gate =============================================

        #region Edit

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody]GateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateErrors());
            }
            model.UserName = null;

            using (var response = await _httpService.PutAsync($"Edit?id={model.Id}", model, token: HttpContext.Session.GetString("Token")))
            {
                if (response.IsSuccessStatusCode)
                {
                    return Ok((await response.Content.ReadAsStringAsync()).Replace("\"", ""));
                }
                else
                {

                    return BadRequest(new { message = await response.Content.ReadAsStringAsync(), model = model });

                }
            }

        }

        #endregion

        // ====================================== Delete Gate =============================================

        public async Task<IActionResult> Delete(Guid id)
        {

            using (var response = await _httpService.DeleteAsync($"Delete?id={id}", HttpContext.Session.GetString("Token")))
            {

                return response.IsSuccessStatusCode
                    ? Ok((await response.Content.ReadAsStringAsync()).Replace("\"", ""))
                    : (IActionResult)BadRequest((await response.Content.ReadAsStringAsync()).Replace("\"", ""));
            }






        }
    }
}