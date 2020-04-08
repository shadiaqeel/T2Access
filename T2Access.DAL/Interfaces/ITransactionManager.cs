using System;
using System.Threading.Tasks;

namespace T2Access.DAL
{
    public interface ITransactionManager : IRepository<Transaction>
    {
        Task<Transaction> GetByGateIdAsync(Guid gateId, int status);
        Task UpdateStatusAsync(decimal id);



    }
}
