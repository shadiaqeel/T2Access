using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models;

namespace T2Access.DAL
{
    public interface ITransactionManager : IRepository<UserGateModel, decimal>
    {
        TransactionModel GetByGateId(Guid gateId , int status);
        bool UpdateStatus(decimal id);

    }
}
