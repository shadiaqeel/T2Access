using System.Collections.Generic;
using System.Linq;

using T2Access.DAL;
using T2Access.Models;

namespace T2Access.BLL.Extensions
{
    public static class GateExtension
    {

        #region Mapper
        public static Gate ToEntity(this IGateModel model)
        {
            return new Gate
            {

                Id = model.Id,
                UserName = model.UserName,
                Password = model.Password,
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                Status = model.Status

            };
        }

        public static IGateModel ToModel(this Gate gate)
        {

            return new GateModel
            {

                Id = gate.Id,
                UserName = gate.UserName,
                NameAr = gate.NameAr,
                NameEn = gate.NameEn,
                Status = (int)gate.Status

            };

        }


        public static IList<IGateModel> ToModel(this IList<Gate> gates)
        {

            return gates.Select(c => c.ToModel()).ToList<IGateModel>();

        }
        #endregion
    }
}
