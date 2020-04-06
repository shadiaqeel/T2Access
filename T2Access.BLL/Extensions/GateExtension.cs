using System.Collections.Generic;
using System.Linq;

using T2Access.DAL;
using T2Access.Models;
using T2Access.Models.Dtos;

namespace T2Access.BLL.Extensions
{
    public static class GateExtension
    {

        #region Mapper



        public static Gate ToEntity(this SignUpGateModel gate)
        {
            return new Gate
            {
                UserName = gate.UserName,
                NameAr = gate.NameAr,
                NameEn = gate.NameEn,
                Password = gate.Password

            };
        }

        public static Gate ToEntity(this GateModel model)
        {
            return new Gate
            {

                Id = model.Id,
                UserName = model.UserName,
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                Status = model.Status

            };
        }

        public static GateDto ToDto(this Gate gate)
        {
            return new GateDto
            {

                Id = gate.Id,
                UserName = gate.UserName,
                NameAr = gate.NameAr,
                NameEn = gate.NameEn,
                Status = (int)gate.Status

            };
        }

        public static IEnumerable<GateDto> ToDto(this IEnumerable<Gate> gates)
        {
            return gates.Select(c => c.ToDto());
        }
        #endregion
    }
}
