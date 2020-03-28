using System;
using System.Collections.Generic;
using T2Access.Models;

namespace T2Access.DAL
{
    public interface IGateManager :IRepository<GateSignUpModel,Guid,GateModel>
    {
        ResponseFilteredGateList GetWithFilter(GateFilterModel gate);
        List<CheckedGateModel> GetCheckedByUserId(Guid userId);
        GateModel GetByUserName(string username);
        GateModel Login(LoginModel gate);
        bool ResetPassword(ResetPasswordModel model);




    }
}
