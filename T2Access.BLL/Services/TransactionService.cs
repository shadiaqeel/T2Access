using System;
using System.Diagnostics;

using T2Access.BLL.Extensions;
using T2Access.DAL;
using T2Access.DAL.Helper;
using T2Access.Models;


namespace T2Access.BLL.Services
{


    public class TransactionService : ITransactionService
    {
        private readonly ITransactionManager transactionManager = ManagerFactory.GetTransactionManager(Variables.DatabaseProvider);



        public bool Create(UserGateModel userGate)
        {
            return ValidUserGate(userGate.ToEntity())
                ? transactionManager.Create(new Transaction() { UserId = userGate.UserId, GateId = userGate.GateId }) == null ? false : true
                : false;
        }

        public TransactionModel GetByGateId(Guid gateId)
        {
            return transactionManager.GetByGateId(gateId, 0).ToModel();
        }

        public bool UpdateStatus(decimal id)
        {
            try
            {
                transactionManager.UpdateStatus(id);

            }
            catch (Exception e)
            {
                Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");

                return false;
            }

            return true;
        }

        public bool ValidUserGate(UserGate userGate)
        {
            IUserGateManager userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);

            return userGateManager.CheckIfExist(userGate);
        }
    }
}
