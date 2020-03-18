using System;
using System.Collections.Generic;
using T2Access.Models;

namespace T2Access.DAL
{
    public interface IGateManager :IRepository<GateSignUpModel,Guid>
    {
        List<GateModel> GetWithFilter(GateFilterModel gate);
        GateModel GetByUserName(string username);
        GateModel Login(LoginModel gate);




    }
}
