using System;
using System.Collections.Generic;
using T2Access.Models;

namespace T2Access.DAL
{
    public interface IGateManager :IRepository<GateSignUpModel>
    {
        //bool Insert(GateSignUpModel gate);
        List<GateModel> GetWithFilter(GateModel gate);
        GateModel GetByUserName(string username);
        GateModel Login(LoginModel gate);

        int GetStatusById(Guid id);



    }
}
