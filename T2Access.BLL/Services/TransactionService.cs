using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (ValidUserGate(userGate))
            {
                return transactionManager.Create(userGate);
            }
            else
            return false; 
        }

        public TransactionModel GetByGateId(Guid gateId)
        {
            return transactionManager.GetByGateId(gateId, 0);
        }



        public bool UpdateStatus(decimal id)
        {
            return transactionManager.UpdateStatus(id);
        }


        public bool ValidUserGate(UserGateModel userGate)
        {
             IUserGateManager userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);

            return userGateManager.CheckIfExist(userGate);
        }
    }
}
