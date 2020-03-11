using System;
using System.Collections.Generic;
using T2Access.Models;



namespace T2Access.DAL
{
    public interface IUserManager : IRepository<UserSignUpModel>
    {
        //bool Insert(UserModel user);
        List<UserModel> GetWithFilter(UserFilterModel filter);
        UserModel GetByUserName(string userName);

        UserModel Login(LoginModel user);

        int GetStatusById(Guid id);

    }
}
