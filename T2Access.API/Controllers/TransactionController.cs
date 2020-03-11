using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using T2Access.BLL.Services;
using T2Access.Models;
using T2Access.API.Attributes;
using System.Web.Http.Description;

namespace T2Access.API.Controllers
{
    public class TransactionController : BaseController
    {
        ITransactionService transactionService = new TransactionService();


        [HttpGet]
        [CustomAuthorize(Roles = "Gate")]
        [ResponseType(typeof(TransactionModel))]
        public HttpResponseMessage GetByGateId(Guid id)
        {

            var  transaction = transactionService.GetByGateId(id);

            if (transaction != null)
                return Request.CreateResponse(HttpStatusCode.OK, transaction);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);

        }



        [HttpPost]
        [CustomAuthorize(Roles ="User")]
        [ResponseType(typeof(UserGateModel))]
        public HttpResponseMessage Create( UserGateModel userGate)
        {


            if (transactionService.Create(userGate))
                return Request.CreateResponse(HttpStatusCode.OK, userGate);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound , "Create transaction failed");


        }



        [HttpPut]
        [CustomAuthorize(Roles = "Gate")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage UpdateStatus(decimal id) 
        {
            if (transactionService.UpdateStatus(id))
                return Request.CreateResponse(HttpStatusCode.OK, $"the status was update successfully for id {id} ");
            else
                return Request.CreateResponse(HttpStatusCode.NotFound , $"the status was update failed for id {id} ");
        }



    }
}
