using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2Access.DAL;
using T2Access.Models;

namespace T2Access.BLL.Extensions
{
    public static class GateExtension
    {

        #region Mapper
        public static Gate ToEntity(this GateModel model)
        {
            return new Gate
            {

                Id = model.Id,
                UserName = model.UserName,
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                Status = (int)model.Status

            };
        }

        public static GateModel ToModel(this Gate gate)
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
        #endregion
    }
}
