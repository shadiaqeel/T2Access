using System.Data;
using System.Data.SqlClient;
using T2Access.Models;

namespace T2Access.DAL
{
    public class TransactionManager : ITransactionManager
    {
        public bool Insert(UserGate userGate)
        {

            bool resultState = false;

            using (SqlConnection connection = new SqlConnection(Variables.ConnectionString))
            {

                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_Transaction_Insert", connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@userId", userGate.UserId);
                    cmd.Parameters.AddWithValue("@gateId", userGate.GateId);



                    resultState = cmd.ExecuteNonQuery() > 0 ? true : false ;




                }

                connection.Close();

            }

            return resultState;
        }




        public Transaction GetByGateId(string gateId, int status)
        {
            Transaction transaction = new Transaction();


            using (SqlConnection connection = new SqlConnection(Variables.ConnectionString))
            {

                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_Transaction_GetByGateId", connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@gateId", gateId);

                    cmd.Parameters.AddWithValue("@status", status);





                    SqlDataReader reader = cmd.ExecuteReader();



                    if (reader.HasRows && reader.Read())
                    {

                        transaction.Id = reader.GetInt32(0);
                        transaction.UserId = reader.GetString(1);
                        transaction.GateId = reader.GetString(2);
                        transaction.CreatedDate = reader.GetDateTime(3);
                        transaction.Status = reader.GetInt32(4);
                        transaction.StatusDate = reader.GetDateTime(5);

                    }
                    else transaction = null;


                }

                connection.Close();

            }

            return transaction;
        }

        public bool Update(int id)
        {
            bool resultState = false;


            using (SqlConnection connection = new SqlConnection(Variables.ConnectionString))
            {

                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_Transaction_Update", connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", id);

                    resultState = cmd.ExecuteNonQuery() > 0 ? true : false;

                }

                connection.Close();

            }
            return resultState;
        }
    }
}
