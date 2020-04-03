using T2Access.DAL;
using T2Access.Models;

namespace T2Access.BLL.Extensions
{
    public static class UserGateExtension
    {

        #region mapper
        public static UserGate ToEntity(this UserGateModel model) => new UserGate
        {

            UserId = model.UserId,
            GateId = model.GateId

        };

        public static UserGateModel ToModel(this UserGate userGate) => new UserGateModel
        {

            UserId = userGate.UserId,
            GateId = userGate.GateId

        };


        #endregion
    }
}
