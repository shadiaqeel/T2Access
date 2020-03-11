using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models;

namespace T2Access.BLL.Services
{
    public interface ITransactionService
    {

        bool Create(UserGateModel userGate);
        TransactionModel GetByGateId(Guid gateId);
        bool UpdateStatus(decimal id);

        bool ValidUserGate(UserGateModel userGate);




    }
}
