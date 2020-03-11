using T2Access.Models;

namespace T2Access.DAL
{
    public interface IUserGateManager : IRepository<UserGateModel>
    {
        // bool Insert(Guid userId, Guid gateId);
        bool Delete(UserGateModel userGate);
        bool CheckIfValid(UserGateModel userGate);
    }
}
