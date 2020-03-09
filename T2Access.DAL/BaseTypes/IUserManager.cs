using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models;



namespace T2Access.DAL
{
    public interface IUserManager : IRepository<User>
    {
        bool Insert(User user);
        List<User> GetWithFilter(User user);




    }
}
