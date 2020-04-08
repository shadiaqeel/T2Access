using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using T2Access.Models;
using T2Access.Models.Dtos;

namespace T2Access.DAL
{
    public interface IGateManager : IRepository<Gate>
    {
        Task<IEnumerable<Gate>> GetWithFilterAsync(Gate gate);
        Task<IEnumerable<CheckedGateDto>> GetCheckedByUserIdAsync(Guid userId);
        Task<Gate> GetByUserNameAsync(string username);
        Task<Gate> LoginAsync(IAuthModel gate);
        Task ResetPasswordAsync(IAuthModel model);




    }
}
