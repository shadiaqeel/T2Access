using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using T2Access.API.Resources;
using T2Access.BLL.Services;
using T2Access.Models;

namespace T2Access.API.Controllers
{
    public class TransactionController : ApiBaseController
    {
        private readonly ITransactionService transactionService;

        //! ===========================================================================

        public TransactionController(ITransactionService transactionService = null)
        {
            this.transactionService = transactionService ?? new TransactionService();
        }
        //! ===========================================================================

        [HttpGet]
        [Authorize(Roles = "Gate")]
        [Produces(typeof(TransactionModel))]
        public IActionResult GetByGateId(Guid id)
        {

            var transaction = transactionService.GetByGateIdAsync(id);

            return transaction != null
                ? Ok(transaction)
                : (IActionResult)NotFound(Resource.TransactionNotExist);
        }



        [HttpPost]
        [Authorize(Roles = "User")]
        [Produces(typeof(UserGateModel))]
        public async Task<IActionResult> Create(UserGateModel userGate)
        {

            return await transactionService.CreateAsync(userGate)
                ? Ok(userGate)
                : (IActionResult)NotFound(Resource.CreateTransactionFailed);
        }



        [HttpPut]
        [Authorize(Roles = "Gate")]
        [Produces(typeof(string))]
        public async Task<IActionResult> UpdateStatus(decimal id)
        {
            return await transactionService.UpdateStatusAsync(id)
                ? Ok(Resource.StatusUpdateSuccess)
                : (IActionResult)NotFound(Resource.StatusUpdateFail);
        }



    }
}
