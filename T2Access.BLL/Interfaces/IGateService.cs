using System;

using T2Access.Models;
using T2Access.Models.Dtos;

namespace T2Access.BLL.Services
{
    public interface IGateService
    {

        ServiceResponse<string> Create(SignUpGateModel gate);
        ServiceResponse<GateDto> Login(LoginModel gate);
        ServiceResponse<GateListResponse> GetListWithFilter(FilterGateModel filter);
        ServiceResponse<ListResponse<CheckedGateDto>> GetCheckedListByUserId(FilterUserModel filter);
        ServiceResponse<string> Delete(Guid id);
        ServiceResponse<string> Edit(GateModel model);
        ServiceResponse<string> ResetPassword(ResetPasswordModel model);

    }
}
