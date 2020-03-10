using System.Collections.Generic;
using T2Access.Models;



namespace T2Access.DAL
{
    public interface IUserManager : IRepository<User>
    {
        bool Insert(User user);
        List<User> GetWithFilter(User user);
        User GetByUserName(string userName);

        User Login(LoginModel user);

    }
}
