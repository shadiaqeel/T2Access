using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using T2Access.Models;


namespace T2Access.DAL
{

    public class UserManager : IUserManager
    {
        public bool Insert(User user)
        {

            Action<SqlCommand> FillCmd = delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.HashedPassword);
                cmd.Parameters.AddWithValue("@firstname", user.FirstName);
                cmd.Parameters.AddWithValue("@lastname", user.LastName);
            };

            return DatabaseExecuter.ExecuteNonQuery("SP_User_Insert", FillCmd);
        }

        public List<User> GetWithFilter(User user)
        {
            List<User> userList = new List<User>();


            Action<SqlCommand> FillCmd = delegate (SqlCommand cmd)
            {

                if (user.Username != null)
                    cmd.Parameters.AddWithValue("@username", user.Username);

                if (user.FirstName != null)
                    cmd.Parameters.AddWithValue("@firstname", user.FirstName);

                if (user.LastName != null)
                    cmd.Parameters.AddWithValue("@lastname", user.LastName);

                if (user.Status != null)
                    cmd.Parameters.AddWithValue("@status", user.Status);



            };

            Action<SqlDataReader> FillReader = delegate (SqlDataReader reader)
            {
                do
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            User _user = new User();

                            user.Id = reader.GetGuid(0);
                            user.Username = reader.GetString(1);
                            user.HashedPassword = reader.GetString(2);
                            user.FirstName = reader.GetString(3);
                            user.LastName = reader.GetString(4);
                            user.CreatedDate = reader.GetDateTime(5);
                            user.Status = reader.GetInt32(6);



                            userList.Add(_user);

                        }


                    }
                    else userList = null;

                } while (reader.NextResult());


            };



            return userList;


        }





    }
}
