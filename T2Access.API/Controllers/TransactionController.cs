using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using T2Access.BLL.Services;
using T2Access.Models;

namespace T2Access.API.Controllers
{
    public class TransactionController : ApiController
    {
        ITransactionService transactionService = new TransactionService();


        public HttpResponseMessage Get(Guid id)
        {

            Transaction  transaction = transactionService.GetByGateId(id);

            if (transaction != null)
                return Request.CreateResponse(HttpStatusCode.OK, transaction);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);

        }





        [HttpPost]
        public HttpResponseMessage Create([FromBody] UserGate userGate)
        {


            if (transactionService.Create(userGate))
                return Request.CreateResponse(HttpStatusCode.OK);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);


        }

        [HttpPut]
        public HttpResponseMessage Update(int id)
        {
            if (transactionService.UpdateStatus(id))
                return Request.CreateResponse(HttpStatusCode.OK);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }



    }
}
