using System;
using System.Collections.Generic;
using T2Access.Models;



namespace T2Access.DAL
{
    public interface IUserManager : IRepository<User> 
    {
        IList<User> GetWithFilter(User filter);
        User GetByUserName(string userName);
        User GetById(Guid usedId);

        User Login(IAuthModel user);
         bool ResetPassword(IAuthModel user);


    }
}
