using System;
using System.Collections.Generic;

namespace T2Access.DAL
{
    public interface IUserGateManager : IRepository<UserGate>
    {
        bool CheckIfExist(UserGate userGate);
        List<Guid> GetByUserId(Guid userid);
        bool DeleteAllByUserId(Guid userId);
    }
}
