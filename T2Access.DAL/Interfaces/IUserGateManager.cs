using T2Access.Models;

namespace T2Access.DAL
{
    public interface IUserGateManager : IRepository<UserGateModel , UserGateModel,UserGateModel>
    {
        bool CheckIfExist(UserGateModel userGate);
    }
}
