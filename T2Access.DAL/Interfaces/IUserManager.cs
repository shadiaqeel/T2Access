using System;
using System.Collections.Generic;
using T2Access.Models;



namespace T2Access.DAL
{
    public interface IUserManager : IRepository<UserSignUpModel,Guid,UserModel> 
    {
        List<UserModel> GetWithFilter(UserFilterModel filter);
        UserModel GetByUserName(string userName);

        UserModel Login(LoginModel user);
         bool ResetPassword(ResetPasswordModel model);


    }
}
