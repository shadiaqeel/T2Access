using System;
using System.Collections.Generic;
using T2Access.Models;

namespace T2Access.BLL.Services
{
    public interface IUserService
    {
        bool Create(UserSignUpModel user);
        User Login(LoginModel user);
        bool CheckUserName(string userName);

        List<User> GetList(User user);

        bool Assign(UserGateModel userGate);

        bool Unassign(UserGateModel userGate);
    }
}
