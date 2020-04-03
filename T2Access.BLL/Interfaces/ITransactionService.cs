using System;

using T2Access.DAL;
using T2Access.Models;

namespace T2Access.BLL.Services
{
    public interface ITransactionService
    {

        bool Create(UserGateModel userGate);
        TransactionModel GetByGateId(Guid gateId);
        bool UpdateStatus(decimal id);

        bool ValidUserGate(UserGate userGate);




    }
}
