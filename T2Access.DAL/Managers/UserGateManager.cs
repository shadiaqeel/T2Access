using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace T2Access.DAL
{
    public class UserGateManager : IUserGateManager
    {
        public bool Insert(string userId, string gateId)
        {
            bool resultState = false;

            using (SqlConnection connection = new SqlConnection(Variables.ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_UserGate_Insert",connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@userId",userId);
                    cmd.Parameters.AddWithValue("@gateId", gateId);


                    resultState = cmd.ExecuteNonQuery() > 0 ? true : false;




                }
                connection.Close();


            }

            return resultState;
        }

        public bool Delete(string userId, string gateId)
        {
            bool resultState = false;

            using (SqlConnection connection = new SqlConnection(Variables.ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_UserGate_Delete", connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@gateId", gateId);


                    resultState = cmd.ExecuteNonQuery() > 0 ? true : false;




                }
                connection.Close();


            }
            return resultState;
        }
    }
}
