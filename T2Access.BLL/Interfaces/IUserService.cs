using System;
using System.Collections.Generic;
using T2Access.Models;

namespace T2Access.BLL.Services
{
    public interface IUserService
    {
        bool Create(SignUpUserModel user);
        UserDto Login(LoginModel user);
        bool CheckUserName(string userName);
        UserDto GetById(Guid userId);
        UserListResponse GetList(FilterUserModel user);
        bool Assign(UserGateModel userGate);
        bool Unassign(UserGateModel userGate);
        bool Delete(Guid id);

        bool Edit(UpdateUserModel model);
        bool ResetPassword(ResetPasswordModel model);

    }
}
