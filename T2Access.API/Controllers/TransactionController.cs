using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

using T2Access.API.Attributes;
using T2Access.API.Resources;
using T2Access.BLL.Services;
using T2Access.Models;

namespace T2Access.API.Controllers
{
    public class TransactionController : BaseController
    {
        private readonly ITransactionService transactionService = new TransactionService();


        [HttpGet]
        [CustomAuthorize(Roles = "Gate")]
        [ResponseType(typeof(TransactionModel))]
        public HttpResponseMessage GetByGateId(Guid id)
        {

            var transaction = transactionService.GetByGateId(id);

            return transaction != null
                ? Request.CreateResponse(HttpStatusCode.OK, transaction)
                : Request.CreateResponse(HttpStatusCode.NotFound, Resource.TransactionNotExist);
        }



        [HttpPost]
        [CustomAuthorize(Roles = "User")]
        [ResponseType(typeof(UserGateModel))]
        public HttpResponseMessage Create(UserGateModel userGate)
        {

            return transactionService.Create(userGate)
                ? Request.CreateResponse(HttpStatusCode.OK, userGate)
                : Request.CreateResponse(HttpStatusCode.NotFound, Resource.CreateTransactionFailed);
        }



        [HttpPut]
        [CustomAuthorize(Roles = "Gate")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage UpdateStatus(decimal id)
        {
            return transactionService.UpdateStatus(id)
                ? Request.CreateResponse(HttpStatusCode.OK, Resource.StatusUpdateSuccess)
                : Request.CreateResponse(HttpStatusCode.NotFound, Resource.StatusUpdateFail);
        }



    }
}
