using System.Collections.Generic;
using System.Data.SqlClient;
using T2Access.Models;
using T2Access.Security;


namespace T2Access.DAL
{

    public class UserManager : IUserManager
    {
        private IPasswordHasher passwordHasher = new PasswordHasher();


        public bool Insert(User user)
        {


            return DatabaseExecuter.ExecuteNonQuery("SP_User_Insert", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@username", user.UserName);
                cmd.Parameters.AddWithValue("@password", passwordHasher.HashPassword(user.Password));
                cmd.Parameters.AddWithValue("@firstname", user.FirstName);
                cmd.Parameters.AddWithValue("@lastname", user.LastName);
            }) > 0 ? true : false;
        }
        public List<User> GetWithFilter(User user)
        {
            List<User> userList = new List<User>();


            DatabaseExecuter.ExecuteQuery("SP_User_SelectWithFilter", delegate (SqlCommand cmd)
            {

                if (user.UserName != null)
                    cmd.Parameters.AddWithValue("@username", user.UserName);

                if (user.FirstName != null)
                    cmd.Parameters.AddWithValue("@firstname", user.FirstName);

                if (user.LastName != null)
                    cmd.Parameters.AddWithValue("@lastname", user.LastName);

                if (user.Status != null)
                    cmd.Parameters.AddWithValue("@status", user.Status);

            },
            delegate (SqlDataReader reader)
            {

                while (reader.Read())
                {

                    userList.Add(new User()
                    {
                        Id = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        FirstName = reader.GetString(3),
                        LastName = reader.GetString(4),
                        CreatedDate = reader.GetDateTime(5),
                        Status = reader.GetInt32(6)
                    });

                }
            });

            return userList;


        }



        public User GetByUserName(string userName) {

            User user = null;

            DatabaseExecuter.ExecuteQuery("SP_User_SelectByUserName", delegate (SqlCommand cmd)
            {

                if (userName != null)
                    cmd.Parameters.AddWithValue("@username", userName);

            },
            delegate (SqlDataReader reader)
            {

                if (reader.Read())
                {

                    user = new User()
                    {
                        Id = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        FirstName = reader.GetString(3),
                        LastName = reader.GetString(4),
                        CreatedDate = reader.GetDateTime(5),
                        Status = reader.GetInt32(6)
                    };

                }
            });


            return user;


        }


        public User Login(LoginModel userModel)
        {


            User user = GetByUserName(userModel.UserName);

            if (user != null && passwordHasher.VerifyHashedPassword(user.Password, userModel.Password))
            {
                user.Password = "";
                return user;
            }
            else
                return null;

        }
    }
}
