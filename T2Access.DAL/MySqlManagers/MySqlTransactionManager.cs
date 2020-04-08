using System;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace T2Access.DAL
{
    public class MySqlTransactionManager : ITransactionManager
    {
        public async Task<Transaction> CreateAsync(Transaction transaction)
        {

            return await DatabaseExecuter.MySqlExecuteNonQueryAsync("SP_Transaction_Insert", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", transaction.UserId);
                cmd.Parameters.AddWithValue("_gateId", transaction.GateId);
            }) > 0 ? transaction : null;

        }

        public Task UpdateAsync(Transaction editmodel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Transaction entity)
        {
            throw new NotImplementedException();
        }

        //===========================================================================

        public async Task<Transaction> GetByGateIdAsync(Guid gateId, int status)
        {

            Transaction transaction = new Transaction();


            await DatabaseExecuter.MySqlExecuteQueryAsync("SP_Transaction_GetByGateId", delegate (MySqlCommand cmd)
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

        public async Task UpdateStatusAsync(decimal id)
        {

            await DatabaseExecuter.MySqlExecuteNonQueryAsync("SP_Transaction_UpdateStatus", delegate (MySqlCommand cmd)
             {
                 cmd.Parameters.AddWithValue("_Id", id);
             });
        }

    }
}
