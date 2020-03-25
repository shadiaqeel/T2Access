using System;
using System.Collections.Generic;
using T2Access.Models;

namespace T2Access.DAL
{
    public interface IUserGateManager : IRepository<UserGateModel , UserGateModel,UserGateModel>
    {
        bool CheckIfExist(UserGateModel userGate);
        List<Guid> GetByUserId(Guid userid);
        bool DeleteAllByUserId(Guid userId);
    }
}
