using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models;
using T2Access.Models.Dtos;

namespace T2Access.BLL.Services
{
    public interface IGateService
    {

        ServiceResponse<string> Create(SignUpGateModel gate);
        ServiceResponse<GateDto> Login(LoginModel gate);
        ServiceResponse<GateListResponse> GetListWithFilter(FilterGateModel filter);
        ServiceResponse<IList<CheckedGateModel>> GetCheckedListByUserId(Guid userId);
        ServiceResponse<string> Delete(Guid id);
        ServiceResponse<string> Edit(GateModel model);
        ServiceResponse<string> ResetPassword(ResetPasswordModel model);

    }
}
