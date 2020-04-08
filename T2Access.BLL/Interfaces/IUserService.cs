using System;
using System.Threading.Tasks;

using T2Access.Models;

namespace T2Access.BLL.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<string>> CreateAsync(SignUpUserModel user);
        Task<ServiceResponse<UserDto>> LoginAsync(LoginModel user);
        //bool CheckUserName(string userName);
        Task<ServiceResponse<UserDto>> GetByIdAsync(Guid userId);
        Task<ServiceResponse<UserListResponse>> GetListAsync(FilterUserModel user);
        Task<ServiceResponse<string>> AssignAsync(UserGateModel userGate);
        Task<ServiceResponse<string>> UnassignAsync(UserGateModel userGate);
        Task<ServiceResponse<string>> DeleteAsync(Guid id);
        Task<ServiceResponse<string>> EditAsync(UpdateUserModel model);
        Task<ServiceResponse<string>> ResetPasswordAsync(ResetPasswordModel model);
    }
}
