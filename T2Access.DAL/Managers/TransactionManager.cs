using System;
using System.Data.Common;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using T2Access.DAL.Helper;
using T2Access.DAL.Options;

namespace T2Access.DAL
{
    public class TransactionManager : ITransactionManager
    {

        private readonly IDatabaseExecuter databaseExecuter;

        //========================================================================================================

        #region Constructors
        public TransactionManager()
        {
            databaseExecuter = DbExecuterFactory.GetExecuter();
        }
        public TransactionManager(IOptionsMonitor<DALOptions> options)
        {
            databaseExecuter = DbExecuterFactory.GetExecuter(options);
        }
        #endregion
        //========================================================================================================



        public async Task<Transaction> CreateAsync(Transaction transaction)
        {

            return await databaseExecuter.ExecuteNonQueryAsync("SP_Transaction_Insert", delegate (DbCommand cmd)
            {
                cmd.AddParameterWithValue("UserId", transaction.UserId);
                cmd.AddParameterWithValue("GateId", transaction.GateId);
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


            await databaseExecuter.ExecuteQueryAsync("SP_Transaction_GetByGateId", delegate (DbCommand cmd)
             {

                 cmd.AddParameterWithValue("GateId", gateId);
                 cmd.AddParameterWithValue("Status", status);

             }, async delegate (DbDataReader reader)
             {
                 if (await reader.ReadAsync())
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

            await databaseExecuter.ExecuteNonQueryAsync("SP_Transaction_UpdateStatus", delegate (DbCommand cmd)
             {
                 cmd.AddParameterWithValue("Id", id);
             });
        }

    }
}
