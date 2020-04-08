using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using T2Access.Models;



namespace T2Access.DAL
{
    public interface IUserManager : IRepository<User>
    {
        Task<IEnumerable<User>> GetWithFilterAsync(User filter);
        Task<User> GetByUserNameAsync(string userName);
        Task<User> GetByIdAsync(Guid usedId);

        Task<User> LoginAsync(IAuthModel user);
        Task ResetPasswordAsync(IAuthModel user);


    }
}
