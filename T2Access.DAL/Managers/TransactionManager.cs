using System;
using System.Data.SqlClient;
using T2Access.Models;

namespace T2Access.DAL
{
    public class TransactionManager : ITransactionManager
    {
        public bool Insert(UserGateModel userGate)
        {

            return DatabaseExecuter.ExecuteNonQuery("SP_Transaction_Insert", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@userId", userGate.UserId);
                cmd.Parameters.AddWithValue("@gateId", userGate.GateId);
            }) > 0 ? true : false ;

        }

        public TransactionModel GetByGateId(Guid gateId, int status)
        {

            TransactionModel transaction = new TransactionModel();


            DatabaseExecuter.ExecuteQuery("SP_Transaction_GetByGateId", delegate (SqlCommand cmd)
             {

                 cmd.Parameters.AddWithValue("@gateId", gateId);

                 cmd.Parameters.AddWithValue("@status", status);
             }, delegate (SqlDataReader reader)
             {
                 if (reader.Read())
                 {

                     transaction = new TransactionModel()
                     {
                         Id = reader.GetDecimal(0),
                         UserId = reader.GetGuid(1),
                         GateId = reader.GetGuid(2),
                         Status = reader.GetInt32(3),
                         StatusDate = reader.GetDateTime(4)
                     };

                 }
             });


            return transaction;
        }

        public bool Update(decimal id)
        {



            return DatabaseExecuter.ExecuteNonQuery("SP_Transaction_Update", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@Id", id);
            }) > 0 ? true : false;
        }
    }
}
