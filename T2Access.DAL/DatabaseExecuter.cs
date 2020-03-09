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




        public static bool ExecuteNonQuery(string storedProcedure, Action<SqlCommand> FillCmd)
        {

            bool resultState = false;

            using (SqlConnection connection = new SqlConnection(Variables.ConnectionString))
            {

                connection.Open();

                using (SqlCommand cmd = new SqlCommand(storedProcedure, connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;

                    FillCmd(cmd);

                    resultState = cmd.ExecuteNonQuery() > 0 ? true : false;




                }

                connection.Close();

            }


            return resultState;

        }
    }
}
