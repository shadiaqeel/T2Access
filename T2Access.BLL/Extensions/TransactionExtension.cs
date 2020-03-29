using System.Collections.Generic;
using System.Linq;

using T2Access.DAL;
using T2Access.Models;

namespace T2Access.BLL.Extensions
{
    public static class TransactionExtension
    {

        #region Mapper
        public static Transaction ToEntity(this TransactionModel model)
        {
            return new Transaction
            {
                Id = model.Id,
                UserId = model.UserId,
                GateId = model.GateId,
                Status = model.Status,
                StatusDate = model.StatusDate

            };
        }

        public static TransactionModel ToModel(this Transaction transaction)
        {

            return new TransactionModel
            {

                Id = transaction.Id,
                UserId = transaction.UserId,
                GateId = transaction.GateId,
                Status = transaction.Status,
                StatusDate = transaction.StatusDate

            };

        }



        #endregion
    }
}
