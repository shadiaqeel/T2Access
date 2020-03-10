using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using T2Access.Models; 

namespace T2Access.DAL
{
    public static class DatabaseExecuter
    {


        public static void ExecuteQuery(string storedProcedure , Action<SqlCommand> FillCmd , Action<SqlDataReader> FillReader) {





            using (SqlConnection connection = new SqlConnection(Variables.ConnectionString))
            {

                connection.Open();

                using (SqlCommand cmd = new SqlCommand(storedProcedure, connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;


                    FillCmd(cmd);

                   SqlDataReader reader = cmd.ExecuteReader();


                    FillReader(reader);




                }

                connection.Close();

            }


        }




        public static int  ExecuteNonQuery(string storedProcedure, Action<SqlCommand> FillCmd)
        {

            int result = 0;

            using (SqlConnection connection = new SqlConnection(Variables.ConnectionString))
            {

                connection.Open();

                using (SqlCommand cmd = new SqlCommand(storedProcedure, connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;

                    FillCmd(cmd);

                    try
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    { result = -1; }


                }

                connection.Close();

            }


            return result;

        }
    }
}
