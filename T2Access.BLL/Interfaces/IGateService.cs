using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models;

namespace T2Access.BLL.Services
{
    public interface IGateService
    {

        bool Create(GateSignUpModel gate);
        GateModel Login(LoginModel gate);
        bool CheckUserName(string userName);
        GateListResponse GetListWithFilter(GateFilterModel filter);
        IList<CheckedGateModel> GetCheckedListByUserId(Guid userId);
        bool Delete(Guid id);
        bool Edit(GateModel model);
        bool ResetPassword(ResetPasswordModel model);

    }
}
