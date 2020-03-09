using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.Models;
using T2Access.DAL;

namespace T2Access.BLL.Services.Impl
{
    public class UserService : IUserService
    {

        private IUserManager userGate = new UserManager();

        public bool Create(User user)
        {

            return userGate.Insert(user);
        }

        public List<User> List(User user)
        {
            return userGate.GetWithFilter(user);
        }
    }
}
