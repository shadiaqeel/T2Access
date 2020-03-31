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

        bool Create(SignUpGateModel gate);
        GateDto Login(LoginModel gate);
        bool CheckUserName(string userName);
        GateListResponse GetListWithFilter(FilterGateModel filter);
        IList<CheckedGateModel> GetCheckedListByUserId(Guid userId);
        bool Delete(Guid id);
        bool Edit(GateModel model);
        bool ResetPassword(ResetPasswordModel model);

    }
}
