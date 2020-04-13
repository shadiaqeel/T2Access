using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace T2Access.DAL
{
    public interface IDatabaseExecuter
    {

        void ExecuteQuery(string storedProcedure, Action<DbCommand> FillCmd, Action<DbDataReader> FillReader);

        int ExecuteNonQuery(string storedProcedure, Action<DbCommand> FillCmd);



        // Async methods : 
        Task ExecuteQueryAsync(string storedProcedure, Action<DbCommand> FillCmd, Func<DbDataReader, Task> FillReader);

        Task<int> ExecuteNonQueryAsync(string storedProcedure, Action<DbCommand> FillCmd);
    }
}
