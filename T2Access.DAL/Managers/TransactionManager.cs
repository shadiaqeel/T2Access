using System;
using System.Data;
using System.Data.SqlClient;
using T2Access.Models;

namespace T2Access.DAL
{
    public class TransactionManager : ITransactionManager
    {
        public bool Insert(UserGate userGate)
        {


            Action<SqlCommand> FillCmd = delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@userId", userGate.UserId);
                cmd.Parameters.AddWithValue("@gateId", userGate.GateId);
            };


            return DatabaseExecuter.ExecuteNonQuery("SP_Transaction_Insert", FillCmd);

        }




        public Transaction GetByGateId(Guid gateId, int status)
        {
            Transaction transaction = new Transaction();



            Action<SqlCommand> FillCmd = delegate (SqlCommand cmd)
            {

                cmd.Parameters.AddWithValue("@gateId", gateId);

                cmd.Parameters.AddWithValue("@status", status);
            };





            Action<SqlDataReader> FillReader = delegate (SqlDataReader reader)
          {
              if (reader.HasRows && reader.Read())
              {

                  transaction.Id = reader.GetDecimal(0);
                  transaction.UserId = reader.GetGuid(1);
                  transaction.GateId = reader.GetGuid(2);
                  transaction.CreatedDate = reader.GetDateTime(3);
                  transaction.Status = reader.GetInt32(4);
                  transaction.StatusDate = reader.GetDateTime(5);

              }
              else transaction = null;
          };



            DatabaseExecuter.ExecuteQuery("SP_Transaction_GetByGateId",FillCmd,FillReader);
            


            return transaction;
        }



        public bool Update(int id)
        {

            Action<SqlCommand> FillCmd = delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@Id", id);
            };


            return DatabaseExecuter.ExecuteNonQuery("SP_Transaction_Update", FillCmd);
        }
    }
}
