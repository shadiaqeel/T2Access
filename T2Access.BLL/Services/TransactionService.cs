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
        private readonly IUserGateManager userGateManager = new UserGateManager();

        public bool Assign(Guid userId, Guid gateId)
        {

            return userGateManager.Insert(userId, gateId);
        }

        public bool Create(UserGate userGate)
        {
            return transactionManager.Insert(userGate);
        }

        public Transaction GetByGateId(Guid gateId)
        {
            return transactionManager.GetByGateId(gateId, 0);
        }

        public bool Unassign(Guid userId, Guid gateId)
        {
            return userGateManager.Delete(userId, gateId);

        }

        public bool UpdateStatus(int id)
        {
            return transactionManager.Update(id);
        }
    }
}
