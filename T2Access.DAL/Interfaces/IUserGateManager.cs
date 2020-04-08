using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace T2Access.DAL
{
    public interface IUserGateManager : IRepository<UserGate>
    {
        Task<bool> CheckIfExistAsync(UserGate userGate);
        Task<List<Guid>> GetByUserIdAsync(Guid userid);
        Task DeleteAllByUserIdAsync(Guid userId);
    }
}
