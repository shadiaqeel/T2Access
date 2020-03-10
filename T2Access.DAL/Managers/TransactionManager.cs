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




        public Transaction GetByGateId(Guid gateId, int status)
        {

            Transaction transaction = new Transaction();


            DatabaseExecuter.ExecuteQuery("SP_Transaction_GetByGateId", delegate (SqlCommand cmd)
             {

                 cmd.Parameters.AddWithValue("@gateId", gateId);

                 cmd.Parameters.AddWithValue("@status", status);
             }, delegate (SqlDataReader reader)
             {
                 if (reader.Read())
                 {

                     transaction = new Transaction()
                     {
                         Id = reader.GetDecimal(0),
                         UserId = reader.GetGuid(1),
                         GateId = reader.GetGuid(2),
                         CreatedDate = reader.GetDateTime(3),
                         Status = reader.GetInt32(4),
                         StatusDate = reader.GetDateTime(5)
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
