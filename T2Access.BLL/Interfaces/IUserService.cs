using System;
using System.Collections.Generic;
using T2Access.Models;

namespace T2Access.BLL.Services
{
    public interface IUserService
    {
        bool Create(UserSignUpModel user);
        UserModel Login(LoginModel user);
        bool CheckUserName(string userName);
        List<UserModel> GetList(UserFilterModel user);
        bool Assign(UserGateModel userGate);
        bool Unassign(UserGateModel userGate);
        bool Delete(Guid id);
    }
}
