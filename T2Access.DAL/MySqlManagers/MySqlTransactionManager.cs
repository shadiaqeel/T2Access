using System;

using MySql.Data.MySqlClient;

namespace T2Access.DAL
{
    public class MySqlTransactionManager : ITransactionManager
    {
        public Transaction Create(Transaction transaction)
        {

            return DatabaseExecuter.MySqlExecuteNonQuery("SP_Transaction_Insert", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", transaction.UserId);
                cmd.Parameters.AddWithValue("_gateId", transaction.GateId);
            }) > 0 ? transaction : null;

        }

        public Transaction GetByGateId(Guid gateId, int status)
        {

            Transaction transaction = new Transaction();


            DatabaseExecuter.MySqlExecuteQuery("SP_Transaction_GetByGateId", delegate (MySqlCommand cmd)
             {

                 cmd.Parameters.AddWithValue("_gateId", gateId);
                 cmd.Parameters.AddWithValue("_status", status);

             }, delegate (MySqlDataReader reader)
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

        public bool UpdateStatus(decimal id)
        {



            return DatabaseExecuter.MySqlExecuteNonQuery("SP_Transaction_UpdateStatus", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_Id", id);
            }) > 0 ? true : false;
        }



        public bool Update(Transaction editmodel)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Transaction entity)
        {
            throw new NotImplementedException();
        }
    }
}
