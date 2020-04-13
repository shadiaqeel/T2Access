using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace T2Access.DAL.DbExecuter
{
    internal class MySqlDatabaseExecuter : IDatabaseExecuter
    {


        public void ExecuteQuery(string storedProcedure, Action<DbCommand> FillCmd, Action<DbDataReader> FillReader)
        {





            using (MySqlConnection connection = new MySqlConnection(Variables.MYSQLConnectionString))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(storedProcedure, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Clear();


                    FillCmd(cmd);

                    MySqlDataReader reader = cmd.ExecuteReader();


                    FillReader(reader);
                }

                connection.Close();

            }


        }

        public int ExecuteNonQuery(string storedProcedure, Action<DbCommand> FillCmd)
        {

            int result = 0;

            using (MySqlConnection connection = new MySqlConnection(Variables.MYSQLConnectionString))
            {

                connection.Open();

                using (MySqlCommand cmd = new MySqlCommand(storedProcedure, connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();


                    FillCmd(cmd);


                    result = cmd.ExecuteNonQuery();




                }

                connection.Close();

            }

            ////if (result == 0) throw new Exception("No rows affected");
            if (result == 0)
            {
                System.Diagnostics.Trace.WriteLine($"{nameof(ExecuteNonQuery)} : No rows affected");
            }

            return result;

        }





        // Async methods : 

        public async Task ExecuteQueryAsync(string storedProcedure, Action<DbCommand> FillCmd, Func<DbDataReader, Task> FillReader)
        {


            using (MySqlConnection connection = new MySqlConnection(Variables.MYSQLConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(storedProcedure, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Clear();


                    FillCmd(cmd);

                    using (var reader = await MySqlExecuteReaderAsync(cmd))
                    {
                        await FillReader(reader);
                        reader.Close();
                    }

                }

                connection.Close();

            }


        }

        public async Task<int> ExecuteNonQueryAsync(string storedProcedure, Action<DbCommand> FillCmd)
        {

            int result = 0;

            using (MySqlConnection connection = new MySqlConnection(Variables.MYSQLConnectionString))
            {

                await connection.OpenAsync();

                using (MySqlCommand cmd = new MySqlCommand(storedProcedure, connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();


                    FillCmd(cmd);


                    result = await cmd.ExecuteNonQueryAsync();


                }

                connection.Close();

            }

            ////if (result == 0) throw new Exception("No rows affected");
            if (result == 0)
            {
                System.Diagnostics.Trace.WriteLine($"{nameof(ExecuteNonQueryAsync)} : No rows affected");
            }

            return result;

        }



        // MySqlCommand Extension 

        private Task<MySqlDataReader> MySqlExecuteReaderAsync(MySqlCommand cmd, CancellationToken cancellationToken = new CancellationToken())
        {
            TaskCompletionSource<MySqlDataReader> taskCompletionSource = new TaskCompletionSource<MySqlDataReader>();
            if (cancellationToken == CancellationToken.None || !cancellationToken.IsCancellationRequested)
                try
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    taskCompletionSource.SetResult(reader);
                }
                catch (Exception exception)
                {
                    taskCompletionSource.SetException(exception);
                }
            else
                taskCompletionSource.SetCanceled();
            return taskCompletionSource.Task;
        }
    }
}
