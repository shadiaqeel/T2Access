using T2Access.Models;

namespace T2Access.DAL
{
    public interface IUserGateManager : IRepository<UserGateModel>
    {
        bool Delete(UserGateModel userGate);
        bool CheckIfExist(UserGateModel userGate);
    }
}
