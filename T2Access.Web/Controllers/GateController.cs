﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
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

    public class GateController : WebController
    {


        IHttpClientService httpService = new HttpClientService(new Uri(Variables.ServerBaseAddress + "gate/"));


        // GET: Gate
        public ActionResult Index()
        {
            return View();
        }





        #region Admin

        #region Accounts Managment

        #region Gate Managment


        public ActionResult GateManagment()
        {

                return View();
        }



        public async Task<ActionResult> GetAll()
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
            var NameAr = Request["columns[1][search][value]"];
            var NameEn = Request["columns[2][search][value]"];
            var status = Request["columns[3][search][value]"];
            var order = $"{sortColumnName} {sortDirection}";

            var response = await httpService.GetAsync($"GetListWithFilter?UserName={userName}&NameAr={NameAr}&NameEn={NameEn}&Status={status}&PageSize={length}&Skip={start}&Order={order}", token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)
            {
                var gates = await response.Content.ReadAsAsync <ListResponse< GateViewModel>>();

                if (Request.IsAjaxRequest())
                {
                    




                    int totalrowsafterfiltering = gates.ResponseList.Count;








                    return Json(new { data = JsonConvert.SerializeObject(gates.ResponseList, new T2Access.Web.Helper.DisplayEnumConverter()) , draw = Request["draw"], recordsTotal = gates.totalEntities  }, JsonRequestBehavior.AllowGet);
                }

                return PartialView(gates);
            }

            return PartialView();
        }





        #region Create Gate Account
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

            var response = await httpService.PostAsync("Signup/", model, token: (string)Session["Token"]);
            var result = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)

                return Json(new { success = true, message = result.Split('\"') });
            else
            {


                var error = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = error;



                return PartialView("_Create", model);


            }

        }
        #endregion



        #region Delete
        public async Task<ActionResult> Delete(Guid id)
        {




            var response = await httpService.DeleteAsync($"Delete?id={id}", (string)Session["Token"]);

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                return Json(new { success = true, message = result.Split('\"') }, JsonRequestBehavior.AllowGet) ;

            }
            else
            {

                return Json(new { success = false, message = result.Split('\"') }, JsonRequestBehavior.AllowGet);

            }




        }
        #endregion


        #region Edit

        public ActionResult Edit()
        {

            return PartialView("_Edit");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( GateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit");
            }
            model.UserName = null;
            var response = await httpService.PutAsync($"Edit?id={model.Id}", model, token: (string)Session["Token"]);
            var result = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)

                return Json(new { success = true, message = result.Split('\"') });
            else
            {


                var error = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = error;

               // ModelState.AddModelError("UserName", error);


                return PartialView("_Edit", model);


            }

        }

        #endregion


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




        public ActionResult GetList()
        {

            return PartialView();

        }





        public async Task<ActionResult> GetFilterd()
        {
            var response = await httpService.GetAsync($"GetListWithFilter?Status={0}", token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode && Request.IsAjaxRequest())
            {
                var filterdGates = await response.Content.ReadAsAsync<ListResponse<GateViewModel>>();


                //Server Side Parameter
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request[$"columns[{Request["order[0][column]"]}][name]"];
                string sortDirection = Request["order[0][dir]"];


                int totalrows = filterdGates.ResponseList.Count;

                if (!string.IsNullOrEmpty(searchValue))
                {
                    filterdGates.ResponseList = filterdGates.ResponseList.Where(x => x.UserName.ToLower().Contains(searchValue.ToLower())
                                    || x.NameAr.ToLower().Contains(searchValue.ToLower())
                                    || x.NameEn.ToLower().Contains(searchValue.ToLower())
                    ).ToList<GateViewModel>();

                }
                int totalrowsafterfiltering = filterdGates.ResponseList.Count;


                //sorting 
                if (!string.IsNullOrEmpty(sortColumnName))
                    filterdGates.ResponseList = filterdGates.ResponseList.OrderBy($"{sortColumnName} {sortDirection}").ToList<GateViewModel>();


                //view = RenderViewToString(ControllerContext, "_ListBody", filterdGates, true)


                return Json(new {data = filterdGates.ResponseList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = filterdGates.totalEntities }, JsonRequestBehavior.AllowGet);
            }

            return null;
        }


        public async Task<ActionResult> GetCheckedByUserId(Guid userId)
        {
            var response = await httpService.GetAsync($"GetCheckedListByUserId/?userId={userId}", token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode && Request.IsAjaxRequest())
            {
                var CheckedGates = await response.Content.ReadAsAsync<List<GateViewModel>>();

                //Server Side Parameter
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request[$"columns[{Request["order[0][column]"]}][name]"];
                string sortDirection = Request["order[0][dir]"];


                int totalrows = CheckedGates.Count;

                if (!string.IsNullOrEmpty(searchValue))
                {
                    CheckedGates = CheckedGates.Where(x => x.UserName.ToLower().Contains(searchValue.ToLower())
                                    || x.NameAr.ToLower().Contains(searchValue.ToLower())
                                    || x.NameEn.ToLower().Contains(searchValue.ToLower())
                    ).ToList<GateViewModel>();

                }
                int totalrowsafterfiltering = CheckedGates.Count;


                //sorting 
                if (!string.IsNullOrEmpty(sortColumnName))
                    CheckedGates = CheckedGates.OrderBy($"{sortColumnName} {sortDirection}").ToList<GateViewModel>();


                // view = RenderViewToString(ControllerContext, "_ListBody", CheckedGates, true)
                return Json(new {data = CheckedGates, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }


            return null;
        }





        #endregion


    }
}