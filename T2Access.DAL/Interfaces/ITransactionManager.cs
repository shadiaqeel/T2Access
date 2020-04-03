using System;

namespace T2Access.DAL
{
    public interface ITransactionManager : IRepository<Transaction>
    {
        Transaction GetByGateId(Guid gateId, int status);
        bool UpdateStatus(decimal id);



    }
}
