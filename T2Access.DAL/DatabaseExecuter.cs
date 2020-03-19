using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace T2Access.DAL
{
    public static class DatabaseExecuter
    {


        public static void ExecuteQuery(string storedProcedure, Action<SqlCommand> FillCmd, Action<SqlDataReader> FillReader)
        {





            using (SqlConnection connection = new SqlConnection(Variables.ConnectionString))
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




        public static int ExecuteNonQuery(string storedProcedure, Action<SqlCommand> FillCmd)
        {

            int result = 0;

            using (SqlConnection connection = new SqlConnection(Variables.ConnectionString))
            {

                connection.Open();

                using (SqlCommand cmd = new SqlCommand(storedProcedure, connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();


                    FillCmd(cmd);

                    try
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result = -2;
                    }


                }

                connection.Close();

            }


            return result;

        }





        //----------------------------------------------------------------------------------------------
        //--------------------------------------MYSQL---------------------------------------------------
        //----------------------------------------------------------------------------------------------




        public static void MySqlExecuteQuery(string storedProcedure, Action<MySqlCommand> FillCmd, Action<MySqlDataReader> FillReader)
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



        public static int MySqlExecuteNonQuery(string storedProcedure, Action<MySqlCommand> FillCmd)
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

                    try
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {


                        result = -2;
                    }


                }

                connection.Close();

            }


            return result;

        }

    }
}
