using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models;

namespace T2Access.DAL
{
    public interface IUserGateManager : IRepository<UserGate>
    {
         bool Insert(string userId, string gateId);
         bool Delete(string userId, string gateId);
    }
}
