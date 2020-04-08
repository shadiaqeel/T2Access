using System;
using System.Threading.Tasks;

using T2Access.Models;
using T2Access.Models.Dtos;

namespace T2Access.BLL.Services
{
    public interface IGateService
    {

        Task<ServiceResponse<string>> CreateAsync(SignUpGateModel gate);
        Task<ServiceResponse<GateDto>> LoginAsync(LoginModel gate);
        Task<ServiceResponse<GateListResponse>> GetListWithFilterAsync(FilterGateModel filter);
        Task<ServiceResponse<ListResponse<CheckedGateDto>>> GetCheckedListByUserIdAsync(FilterUserModel filter);
        Task<ServiceResponse<string>> DeleteAsync(Guid id);
        Task<ServiceResponse<string>> EditAsync(GateModel model);
        Task<ServiceResponse<string>> ResetPasswordAsync(ResetPasswordModel model);

    }
}
