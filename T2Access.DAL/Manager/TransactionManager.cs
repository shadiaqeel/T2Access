using System;

using System.Data.SqlClient;

namespace T2Access.DAL
{
    public class TransactionManager : ITransactionManager
    {
        public Transaction Create(Transaction transaction)
        {

            return DatabaseExecuter.ExecuteNonQuery("SP_Transaction_Insert", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", transaction.UserId);
                cmd.Parameters.AddWithValue("_gateId", transaction.GateId);
            }) > 0 ? transaction : null;

        }

        public Transaction GetByGateId(Guid gateId, int status)
        {

            Transaction transaction = new Transaction();


            DatabaseExecuter.ExecuteQuery("SP_Transaction_GetByGateId", delegate (SqlCommand cmd)
             {

                 cmd.Parameters.AddWithValue("_gateId", gateId);
                 cmd.Parameters.AddWithValue("_status", status);

             }, delegate (SqlDataReader reader)
             {
                 if (reader.Read())
                 {

                     transaction = new Transaction()
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

        public void UpdateStatus(decimal id)
        {

            DatabaseExecuter.ExecuteNonQuery("SP_Transaction_UpdateStatus", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_Id", id);
            });
        }

        public void Update(Transaction editmodel)
        {
            throw new NotImplementedException();
        }

        public void Delete(Transaction entity)
        {
            throw new NotImplementedException();
        }
    }
}
