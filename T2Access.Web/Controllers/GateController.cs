using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using T2Access.Models;
using T2Access.Models.Dtos;
using T2Access.Services.HttpClientService;
using T2Access.Web.Models;

namespace T2Access.Web.Controllers
{

    [Authorize]

    public class GateController : WebController
    {
        private readonly IHttpClientService _httpService ;
        private readonly ILogger<GateController> _logger;


        //============================================================================
        public GateController(IHttpClientService httpService , ILogger<GateController> logger)
        {
            _logger = logger;
            _httpService = httpService;
            _httpService.BaseUri = new Uri($"{_httpService.BaseUri}{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}/gate/");
        }
        //============================================================================

        // GET: Gate
        public IActionResult Index => View();

        public IActionResult GateManagment()
        {
            return View();
        }

        public async Task<IActionResult> GetAll([FromBody]DTParameters param)
        {
            
            ////Server Side Parameter
            //var start = Convert.ToInt32(Request.Form["start"]);
            //var length = Convert.ToInt32(Request.Form["length"]);
            //var sortColumnName = Request.Form[$"columns[{Request.Form["order[0][column]"]}][name]"];
            //var sortDirection = Request.Form["order[0][dir]"];
            //////var searchValue = Request.Form["search[value]"];



            ////find search columns info
            //var userName = Request.Form["columns[0][search][value]"];
            //var NameAr = Request.Form["columns[1][search][value]"];
            //var NameEn = Request.Form["columns[2][search][value]"];
            //var status = Request.Form["columns[3][search][value]"];
            //var order = $"{sortColumnName} {sortDirection}";

           // using (var response = await _httpService.GetAsync($"GetListWithFilter?UserName={userName}&NameAr={NameAr}&NameEn={NameEn}&Status={status}&PageSize={length}&Skip={start}&Order={order}", token: HttpContext.Session.GetString("Token")))
            using (var response = await _httpService.GetAsync($"GetListWithFilter?UserName={param.Columns[0].Search.Value}&NameAr={param.Columns[1].Search.Value}&NameEn={param.Columns[2].Search.Value}&Status={param.Columns[3].Search.Value}&PageSize={param.Length}&Skip={param.Start}&Order={param.SortOrder}", token: HttpContext.Session.GetString("Token")))
            {
                if (response.IsSuccessStatusCode)
                {
                    var gates = await response.Content.ReadAsAsync<ListResponse<GateViewModel>>();
                    //var gates = JsonConvert.DeserializeObject<ListResponse<GateViewModel>>(await response.Content.ReadAsStringAsync());


                    return Request.Headers["X-Requested-With"] == "XMLHttpRequest"
                        ? Json(new { data = JsonConvert.SerializeObject(gates.ResponseList, new Helper.DisplayEnumConverter()), draw = Request.Form["draw"], recordsTotal = gates.TotalEntities })
                        : (IActionResult)PartialView(gates);
                }
            }

            return PartialView();
        }


        // ====================================== Create Gate =============================================

        #region Create
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SignUpGateModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create");
            }

            using (var response = await _httpService.PostAsync("Signup/", model, token: (string)HttpContext.Session.GetString("Token")))
            {
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = result.Replace("\"", "") });
                }
                else
                {

                    ViewBag.ErrorMessage = result.Replace("\"", "") ?? null;

                    return PartialView("_Create", model);
                }
            }

        }
        #endregion

        // ====================================== Delete Gate =============================================

        public async Task<IActionResult> Delete(Guid id)
        {

            using (var response = await _httpService.DeleteAsync($"Delete?id={id}", (string)HttpContext.Session.GetString("Token")))
            {

                return response.IsSuccessStatusCode
                    ? Json(new { success = true, message = (await response.Content.ReadAsStringAsync()).Replace("\"", "") })
                    : Json(new { success = false, message = (await response.Content.ReadAsStringAsync()).Replace("\"", "") });
            }

        }



        // ====================================== Edit Gate =============================================

        #region Edit
        public IActionResult Edit()
        {
            return PartialView("_Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit");
            }
            model.UserName = null;
            using var response = await _httpService.PutAsync($"Edit?id={model.Id}", model, token: (string)HttpContext.Session.GetString("Token"));
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = (await response.Content.ReadAsStringAsync()).Replace("\"", "") });
            }
            else
            {

                ViewBag.ErrorMessage = await response.Content.ReadAsStringAsync();

                return PartialView("_Edit", model);
            }

        }
        #endregion


        // ====================================== Reset Password =============================================


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
            HttpContext.Session.SetString("ConfirmedOperation",false.ToString());


            using (var response = await _httpService.PutAsync($"ResetPassword?id={model.Id}", model, token: (string)HttpContext.Session.GetString("Token")))
            {
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    ViewBag.ErrorMessage = await response.Content.ReadAsStringAsync();
                    return PartialView("_ResetPassword", model);
                }
            }

        }
        #endregion

        // ====================================== Get List Gate =============================================

        #region Get List
        public IActionResult GetList()
        {
            return PartialView();
        }

        public async Task<IActionResult> GetFiltered([FromBody]DTParameters param)
        {
            // TODO : ServerSide in GetFiltered method
            using (var response = await _httpService.GetAsync($"GetListWithFilter?UserName={param.Columns[0].Search.Value}&NameAr={param.Columns[1].Search.Value}&NameEn={param.Columns[2].Search.Value}&Status={param.Columns[3].Search.Value}&PageSize={param.Length}&Skip={param.Start}&Order={param.SortOrder}", token: HttpContext.Session.GetString("Token")))
            {
                if (response.IsSuccessStatusCode && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var filterdGates = await response.Content.ReadAsAsync<ListResponse<GateViewModel>>();


                    var totalrows = filterdGates.ResponseList.Count();

                    if (!string.IsNullOrEmpty(param.Search.Value))
                    {
                        filterdGates.ResponseList = filterdGates.ResponseList.Where(x => x.UserName.ToLower().Contains(param.Search.Value.ToLower())
                                        || x.NameAr.ToLower().Contains(param.Search.Value.ToLower())
                                        || x.NameEn.ToLower().Contains(param.Search.Value.ToLower())
                        ).ToList<GateViewModel>();

                    }
                    var totalrowsafterfiltering = filterdGates.ResponseList.Count();


                    //sorting 
                    if (!string.IsNullOrEmpty(param.SortOrder))
                    {
                        //filterdGates.ResponseList = filterdGates.ResponseList.OrderBy(param.SortOrder).ToList<GateViewModel>();
                    }


                    //view = RenderViewToString(ControllerContext, "_ListBody", filterdGates, true)


                    return Json(new { data = filterdGates.ResponseList, draw = Request.Form["draw"], recordsTotal = filterdGates.TotalEntities, recordsFiltered = filterdGates.TotalEntities });
                }
            }

            return null;
        }


        public async Task<IActionResult> GetCheckedByUserId(Guid userId)
        {
            //Server Side Parameter
            var start = Convert.ToInt32(Request.Form["start"]);
            var length = Convert.ToInt32(Request.Form["length"]);
            var searchValue = Request.Form["search[value]"];


            using (var response = await _httpService.GetAsync($"GetCheckedListByUserId?Id={userId}&Skip={start}&PageSize={length}&SearchValue={searchValue}", token: HttpContext.Session.GetString("Token")))
            {
                if (response.IsSuccessStatusCode && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var CheckedGates = await response.Content.ReadAsAsync<ListResponse<CheckedGateDto>>();

                    #region Removed 
                    // int totalrows = CheckedGates.Count;

                    //if (!string.IsNullOrEmpty(searchValue))
                    //{
                    //    CheckedGates = CheckedGates.Where(x => x.UserName.ToLower().Contains(searchValue.ToLower())
                    //                    || x.NameAr.ToLower().Contains(searchValue.ToLower())
                    //                    || x.NameEn.ToLower().Contains(searchValue.ToLower())
                    //    ).ToList<GateViewModel>();

                    //}

                    // int totalrowsafterfiltering = CheckedGates.Count;


                    // //sorting 
                    //if (!string.IsNullOrEmpty(sortColumnName))
                    //{
                    //    CheckedGates = CheckedGates.OrderBy($"{sortColumnName} {sortDirection}").ToList<GateViewModel>();
                    //}


                    // view = RenderViewToString(ControllerContext, "_ListBody", CheckedGates, true) 
                    #endregion

                    return string.IsNullOrEmpty(searchValue)
                        ? Json(new { data = CheckedGates.ResponseList, draw = Request.Form["draw"], recordsTotal = CheckedGates.TotalEntities })
                        : Json(new { data = CheckedGates.ResponseList, draw = Request.Form["draw"], recordsTotal = CheckedGates.ResponseList.Count() });
                }
            }


            return null;
        }
        #endregion







    }
}