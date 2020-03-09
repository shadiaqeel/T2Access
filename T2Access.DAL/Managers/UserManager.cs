using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using T2Access.Models;


namespace T2Access.DAL
{

    public class UserManager : IUserManager
    {
        public bool Insert(User user)
        {
            bool resultState = false;


            using (SqlConnection connection = new SqlConnection(Variables.ConnectionString))
            {

                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_User_Insert", connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@password", user.HashedPassword);
                    cmd.Parameters.AddWithValue("@firstname", user.FirstName);
                    cmd.Parameters.AddWithValue("@lastname", user.LastName);


                    resultState = cmd.ExecuteNonQuery() > 0 ? true : false;




                }

                connection.Close();

            }

            return resultState;
        }

        public List<User> GetWithFilter(User user)
        {
            List<User> userList = new List<User>();


            using (SqlConnection connection = new SqlConnection(Variables.ConnectionString))
            {

                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_User_SelectWithFilter", connection))
                {


                    cmd.CommandType = CommandType.StoredProcedure;

                    if (user.Username != null)
                        cmd.Parameters.AddWithValue("@username", user.Username);

                    if (user.FirstName != null)
                        cmd.Parameters.AddWithValue("@firstname", user.FirstName);

                    if (user.LastName != null)
                        cmd.Parameters.AddWithValue("@lastname", user.LastName);

                    if (user.Status != null)
                        cmd.Parameters.AddWithValue("@status", user.Status);




                    SqlDataReader reader = cmd.ExecuteReader();


                    do
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                User _user = new User();

                                user.Id = reader.GetString(0);
                                user.Username= reader.GetString(1);
                                user.HashedPassword = reader.GetString(2);
                                user.FirstName= reader.GetString(3);
                                user.LastName= reader.GetString(4);
                                user.CreatedDate= reader.GetDateTime(5);
                                user.Status= reader.GetInt32(6);



                                userList.Add(_user);

                            }


                        }
                        else userList = null;

                    } while (reader.NextResult());

                }

                connection.Close();

            }

            return userList;


        }





    }
}
