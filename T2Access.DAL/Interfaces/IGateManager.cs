using System;
using System.Collections.Generic;
using T2Access.Models;

namespace T2Access.DAL
{
    public interface IGateManager :IRepository<Gate>
    {
        IEnumerable<Gate> GetWithFilter(Gate gate);
        IEnumerable<CheckedGateModel> GetCheckedByUserId(Guid userId);
        Gate GetByUserName(string username);
        Gate Login(IAuthModel gate);
        bool ResetPassword(IAuthModel model);




    }
}
