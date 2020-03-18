using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

using T2Access.Models;
using T2Access.Services.HttpClientService;

namespace T2Access.Web.Controllers
{
    public class GateController : Controller
    {


        IHttpClientService httpService = new HttpClientService(new Uri(Variables.ServerBaseAddress));


        // GET: Gate
        public ActionResult Index()
        {
            return View();
        }





        #region Admin

        #region Accounts Managment

        #region User Managment


        public async Task<ActionResult> GateManagment()
        {

            var response = await httpService.GetAsync("gate/GetListWithFilter/", token: (string)Session["Token"]);


            if (response.IsSuccessStatusCode)
            {
                var Gates = await response.Content.ReadAsAsync<List<GateModel>>();

                return View(Gates);
            }
            else
                return View();
        }



        #endregion
        #endregion
        #endregion


    }
}