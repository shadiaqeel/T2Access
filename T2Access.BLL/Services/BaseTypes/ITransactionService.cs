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

        bool Create(UserGate userGate);
        Transaction GetByGateId(Guid gateId);
        bool UpdateStatus(int id);
        bool Assign(Guid userId, Guid gateId);

        bool Unassign(Guid userId, Guid gateId);





    }
}
