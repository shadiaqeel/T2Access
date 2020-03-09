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
        Transaction GetByGateId(string gateId);
        bool UpdateStatus(int id);
        bool Assign(string userId, string gateId);

        bool Unassign(string userId, string gateId);





    }
}
