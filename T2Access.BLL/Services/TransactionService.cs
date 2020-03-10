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
            return transactionManager.Insert(userGate);
        }

        public Transaction GetByGateId(Guid gateId)
        {
            return transactionManager.GetByGateId(gateId, 0);
        }



        public bool UpdateStatus(decimal id)
        {
            return transactionManager.Update(id);
        }
    }
}
