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




        [HttpPost]
        public async Task<IActionResult> LoadData([FromBody]DTGateParameters param)
        {
            // TODO : ServerSide in GetFiltered method
            using (var response = await _httpService.GetAsync($"GetListWithFilter?NameAr={param.Filter?.Namear}&NameEn={param.Filter?.Nameen}&PageSize={param.Length}&Skip={param.Start}", token: HttpContext.Session.GetString("Token")))  // &Order={param.SortOrder}
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

            return BadRequest();
        }


       
        }
}