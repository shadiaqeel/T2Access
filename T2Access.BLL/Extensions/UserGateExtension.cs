using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.DAL;
using T2Access.Models;

namespace T2Access.BLL.Extensions
{
    public static class UserGateExtension
    {

        #region mapper
        public static UserGate ToEntity(this UserGateModel model)
        {
            return new UserGate
            {

                UserId = model.UserId,
                GateId = model.GateId

            };
        }

        public static UserGateModel ToModel(this UserGate userGate)
        {

            return new UserGateModel
            {

                UserId = userGate.UserId,
                GateId = userGate.GateId

            };
        }


        #endregion
    }
}
