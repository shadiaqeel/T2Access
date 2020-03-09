using System.Collections.Generic;
using T2Access.Models;

namespace T2Access.BLL.Services
{
    public interface IUserService
    {
        bool Create(User user);
        List<User> List(User user);
    }
}
