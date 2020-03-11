using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.DAL;
using T2Access.Models;

namespace T2Access.BLL.Services
{


    public class TransactionService : ITransactionService
    {
        private readonly ITransactionManager transactionManager = new TransactionManager();



        public bool Create(UserGateModel userGate)
        {
            if (ValidUserGate(userGate))
            {
                return transactionManager.Insert(userGate);
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
            return transactionManager.Update(id);
        }


        public bool ValidUserGate(UserGateModel userGate)
        {
             IUserGateManager userGateManager = new UserGateManager();



            //if (userManager.GetStatusById(userGate.UserId) ==0 && gateManager.GetStatusById(userGate.GateId) ==0 && )

            return userGateManager.CheckIfValid(userGate);
        }
    }
}
