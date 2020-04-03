using System;

using T2Access.Models;

namespace T2Access.BLL.Services
{
    public interface IUserService
    {
        ServiceResponse<string> Create(SignUpUserModel user);
        ServiceResponse<UserDto> Login(LoginModel user);
        //bool CheckUserName(string userName);
        ServiceResponse<UserDto> GetById(Guid userId);
        ServiceResponse<UserListResponse> GetList(FilterUserModel user);
        ServiceResponse<string> Assign(UserGateModel userGate);
        ServiceResponse<string> Unassign(UserGateModel userGate);
        ServiceResponse<string> Delete(Guid id);
        ServiceResponse<string> Edit(UpdateUserModel model);
        ServiceResponse<string> ResetPassword(ResetPasswordModel model);
    }
}
