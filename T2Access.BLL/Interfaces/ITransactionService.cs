using System;
using System.Threading.Tasks;

using T2Access.Models;

namespace T2Access.BLL.Services
{
    public interface ITransactionService
    {

        Task<bool> CreateAsync(UserGateModel userGate);
        Task<TransactionModel> GetByGateIdAsync(Guid gateId);
        Task<bool> UpdateStatusAsync(decimal id);
        // bool ValidUserGate(UserGate userGate);




    }
}
