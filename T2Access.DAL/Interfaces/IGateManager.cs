using System;
using System.Collections.Generic;

using T2Access.Models;
using T2Access.Models.Dtos;

namespace T2Access.DAL
{
    public interface IGateManager : IRepository<Gate>
    {
        IEnumerable<Gate> GetWithFilter(Gate gate);
        IEnumerable<CheckedGateDto> GetCheckedByUserId(Guid userId);
        Gate GetByUserName(string username);
        Gate Login(IAuthModel gate);
        bool ResetPassword(IAuthModel model);




    }
}
