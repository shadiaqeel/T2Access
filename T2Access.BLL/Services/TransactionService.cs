using System;
using System.Diagnostics;
using System.Threading.Tasks;

using T2Access.BLL.Extensions;
using T2Access.DAL;
using T2Access.DAL.Helper;
using T2Access.Models;


namespace T2Access.BLL.Services
{


    public class TransactionService : ITransactionService
    {
        private readonly ITransactionManager transactionManager;

        public TransactionService(ITransactionManager transactionManager = null)
        {
            this.transactionManager = transactionManager ?? ManagerFactory.GetTransactionManager();
        }


        //==========================================================================

        public async Task<bool> CreateAsync(UserGateModel userGate)
        {
            return await ValidUserGateAsync(userGate.ToEntity())
                ? await transactionManager.CreateAsync(new Transaction() { UserId = userGate.UserId, GateId = userGate.GateId }) == null ? false : true
                : false;
        }

        public async Task<TransactionModel> GetByGateIdAsync(Guid gateId)
        {
            return (await transactionManager.GetByGateIdAsync(gateId, 0)).ToModel();
        }

        public async Task<bool> UpdateStatusAsync(decimal id)
        {
            try
            {
                await transactionManager.UpdateStatusAsync(id);

            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return false;
            }

            return true;
        }

        private async Task<bool> ValidUserGateAsync(UserGate userGate)
        {
            IUserGateManager userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);

            return await userGateManager.CheckIfExistAsync(userGate);
        }
    }
}
