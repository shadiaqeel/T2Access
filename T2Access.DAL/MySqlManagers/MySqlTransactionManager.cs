using System;
using MySql.Data.MySqlClient;
using T2Access.Models;

namespace T2Access.DAL
{
    public class MySqlTransactionManager : ITransactionManager
    {
        public bool Insert(UserGateModel userGate)
        {

            return DatabaseExecuter.MySqlExecuteNonQuery("SP_Transaction_Insert", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userGate.UserId);
                cmd.Parameters.AddWithValue("_gateId", userGate.GateId);
            }) > 0 ? true : false ;

        }

        public TransactionModel GetByGateId(Guid gateId, int status)
        {

            TransactionModel transaction = new TransactionModel();


            DatabaseExecuter.MySqlExecuteQuery("SP_Transaction_GetByGateId", delegate (MySqlCommand cmd)
             {

                 cmd.Parameters.AddWithValue("_gateId", gateId);
                 cmd.Parameters.AddWithValue("_status", status);

             }, delegate (MySqlDataReader reader)
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

        public bool UpdateStatus(decimal id)
        {



            return DatabaseExecuter.MySqlExecuteNonQuery("SP_Transaction_UpdateStatus", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_Id", id);
            }) > 0 ? true : false;
        }
    }
}
