using System;
using System.Collections.Generic;
using T2Access.Models;

namespace T2Access.BLL.Services
{
    public interface IUserService
    {
        bool Create(UserSignUpModel user);
        IUserModel Login(LoginModel user);
        bool CheckUserName(string userName);
        IUserModel GetById(Guid userId);
        UserListResponse GetList(UserFilterModel user);
        bool Assign(UserGateModel userGate);
        bool Unassign(UserGateModel userGate);
        bool Delete(Guid id);

        bool Edit(UserUpdateModel model);
        bool ResetPassword(ResetPasswordModel model);

    }
}
