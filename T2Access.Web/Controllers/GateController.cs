using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

using Newtonsoft.Json;

using T2Access.Models;
using T2Access.Models.Dtos;
using T2Access.Services.HttpClientService;
using T2Access.Web.Attributes;

namespace T2Access.Web.Controllers
{

    [CustomAuthorize]

    public class GateController : WebController
    {
        private readonly IHttpClientService httpService = new HttpClientService(new Uri(Variables.ServerBaseAddress + $"{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}/gate/"));


        // GET: Gate
        public ActionResult Index => View();

        public ActionResult GateManagment()
        {
            return View();
        }

        public async Task<ActionResult> GetAll()
        {

            //Server Side Parameter
            var start = Convert.ToInt32(Request["start"]);
            var length = Convert.ToInt32(Request["length"]);
            var sortColumnName = Request[$"columns[{Request["order[0][column]"]}][name]"];
            var sortDirection = Request["order[0][dir]"];
            ////var searchValue = Request["search[value]"];



            //find search columns info
            var userName = Request["columns[0][search][value]"];
            var NameAr = Request["columns[1][search][value]"];
            var NameEn = Request["columns[2][search][value]"];
            var status = Request["columns[3][search][value]"];
            var order = $"{sortColumnName} {sortDirection}";

            using (var response = await httpService.GetAsync($"GetListWithFilter?UserName={userName}&NameAr={NameAr}&NameEn={NameEn}&Status={status}&PageSize={length}&Skip={start}&Order={order}", token: (string)Session["Token"]))
            {
                if (response.IsSuccessStatusCode)
                {
                    var gates = await response.Content.ReadAsAsync<ListResponse<GateViewModel>>();

                    return Request.IsAjaxRequest()
                        ? Json(new { data = JsonConvert.SerializeObject(gates.ResponseList, new T2Access.Web.Helper.DisplayEnumConverter()), draw = Request["draw"], recordsTotal = gates.TotalEntities }, JsonRequestBehavior.AllowGet)
                        : (ActionResult)PartialView(gates);
                }
            }

            return PartialView();
        }


        // ====================================== Create Gate =============================================

        #region Create
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SignUpGateModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create");
            }

            using (var response = await httpService.PostAsync("Signup/", model, token: (string)Session["Token"]))
            {
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = result.Replace("\"", "") });
                }
                else
                {

                    ViewBag.ErrorMessage = result.Replace("\"", "");

                    return PartialView("_Create", model);
                }
            }

        }
        #endregion

        // ====================================== Delete Gate =============================================

        public async Task<ActionResult> Delete(Guid id)
        {

            using (var response = await httpService.DeleteAsync($"Delete?id={id}", (string)Session["Token"]))
            {

                return response.IsSuccessStatusCode
                    ? Json(new { success = true, message = (await response.Content.ReadAsStringAsync()).Replace("\"", "") }, JsonRequestBehavior.AllowGet)
                    : Json(new { success = false, message = (await response.Content.ReadAsStringAsync()).Replace("\"", "") }, JsonRequestBehavior.AllowGet);
            }

        }

        // ====================================== Edit Gate =============================================

        #region Edit
        public ActionResult Edit()
        {
            return PartialView("_Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit");
            }
            model.UserName = null;
            using (var response = await httpService.PutAsync($"Edit?id={model.Id}", model, token: (string)Session["Token"]))
            {


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

        }
        #endregion


        // ====================================== Reset Password =============================================


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
                    ViewBag.ErrorMessage = await response.Content.ReadAsStringAsync();
                    return PartialView("_ResetPassword", model);
                }
            }

        }
        #endregion

        // ====================================== Get List Gate =============================================

        #region Get List
        public ActionResult GetList()
        {
            return PartialView();
        }

        public async Task<ActionResult> GetFiltered()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);

            // TODO : ServerSide in GetFiltered method
            //&PageSize={length}&Skip={start}

            using (var response = await httpService.GetAsync($"GetListWithFilter?Status={0}&PageSize={length}&Skip={start}", token: (string)Session["Token"]))
            {
                if (response.IsSuccessStatusCode && Request.IsAjaxRequest())
                {
                    var filterdGates = await response.Content.ReadAsAsync<ListResponse<GateViewModel>>();

                    //Server Side Parameter
                    string searchValue = Request["search[value]"];
                    string sortColumnName = Request[$"columns[{Request["order[0][column]"]}][name]"];
                    string sortDirection = Request["order[0][dir]"];

                    var totalrows = filterdGates.ResponseList.Count();

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        filterdGates.ResponseList = filterdGates.ResponseList.Where(x => x.UserName.ToLower().Contains(searchValue.ToLower())
                                        || x.NameAr.ToLower().Contains(searchValue.ToLower())
                                        || x.NameEn.ToLower().Contains(searchValue.ToLower())
                        ).ToList<GateViewModel>();

                    }
                    var totalrowsafterfiltering = filterdGates.ResponseList.Count();


                    //sorting 
                    if (!string.IsNullOrEmpty(sortColumnName))
                    {
                        filterdGates.ResponseList = filterdGates.ResponseList.OrderBy($"{sortColumnName} {sortDirection}").ToList<GateViewModel>();
                    }


                    //view = RenderViewToString(ControllerContext, "_ListBody", filterdGates, true)


                    return Json(new { data = filterdGates.ResponseList, draw = Request["draw"], recordsTotal = filterdGates.TotalEntities, recordsFiltered = filterdGates.TotalEntities }, JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }


        public async Task<ActionResult> GetCheckedByUserId(Guid userId)
        {
            //Server Side Parameter
            var start = Convert.ToInt32(Request["start"]);
            var length = Convert.ToInt32(Request["length"]);
            var searchValue = Request["search[value]"];


            using (var response = await httpService.GetAsync($"GetCheckedListByUserId?Id={userId}&Skip={start}&PageSize={length}&SearchValue={searchValue}", token: (string)Session["Token"]))
            {
                if (response.IsSuccessStatusCode && Request.IsAjaxRequest())
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
                        ? Json(new { data = CheckedGates.ResponseList, draw = Request["draw"], recordsTotal = CheckedGates.TotalEntities }, JsonRequestBehavior.AllowGet)
                        : Json(new { data = CheckedGates.ResponseList, draw = Request["draw"], recordsTotal = CheckedGates.ResponseList.Count() }, JsonRequestBehavior.AllowGet);
                }
            }


            return null;
        }
        #endregion







    }
}