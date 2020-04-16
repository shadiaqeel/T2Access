using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace T2Access.DAL.DbExecuter
{
    internal class SqlDatabaseExecuter : IDatabaseExecuter
    {
        private readonly string connectionString;

        public SqlDatabaseExecuter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public SqlDatabaseExecuter()
        {
        }

        public void ExecuteQuery(string storedProcedure, Action<DbCommand> FillCmd, Action<DbDataReader> FillReader)
        {





            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();


                using (SqlCommand cmd = new SqlCommand(storedProcedure, connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Clear();


                    FillCmd(cmd);

                    SqlDataReader reader = cmd.ExecuteReader();


                    FillReader(reader);




                }

                connection.Close();

            }


        }

        public int ExecuteNonQuery(string storedProcedure, Action<DbCommand> FillCmd)
        {

            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                using (SqlCommand cmd = new SqlCommand(storedProcedure, connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();


                    FillCmd(cmd);


                    result = cmd.ExecuteNonQuery();



                }

                connection.Close();

            }


            return result;

        }


        // Async methods :

        public async Task ExecuteQueryAsync(string storedProcedure, Action<DbCommand> FillCmd, Func<DbDataReader, Task> FillReader)
        {



            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                await connection.OpenAsync();


                using (SqlCommand cmd = new SqlCommand(storedProcedure, connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Clear();


                    FillCmd(cmd);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {


                        //FillReader(reader);
                        await FillReader(reader);
                        //await Task.Run( async () => FillReader(reader));

                        reader.Close();


                    }




                }

                connection.Close();

            }


        }

        public async Task<int> ExecuteNonQueryAsync(string storedProcedure, Action<DbCommand> FillCmd)
        {

            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                await connection.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(storedProcedure, connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();


                    FillCmd(cmd);


                    result = await cmd.ExecuteNonQueryAsync();



                }

                connection.Close();

            }


            return result;

        }
    }
}
