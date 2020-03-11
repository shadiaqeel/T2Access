using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using T2Access.Models;
using T2Access.Security;


namespace T2Access.DAL
{

    public class UserManager : IUserManager
    {
        private IPasswordHasher passwordHasher = new PasswordHasher();


        public bool Insert(UserSignUpModel user)
        {


            return DatabaseExecuter.ExecuteNonQuery("SP_User_Insert", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@username", user.UserName);
                cmd.Parameters.AddWithValue("@password", passwordHasher.HashPassword(user.Password));
                cmd.Parameters.AddWithValue("@firstname", user.FirstName);
                cmd.Parameters.AddWithValue("@lastname", user.LastName);
            }) > 0 ? true : false;

        }
    
        
        public List<UserModel> GetWithFilter(UserFilterModel filter)
        {
            List<UserModel> userList = new List<UserModel>();


            DatabaseExecuter.ExecuteQuery("SP_User_SelectWithFilter", delegate (SqlCommand cmd)
            {

                if (filter.UserName != null)
                    cmd.Parameters.AddWithValue("@username", filter.UserName);

                if (filter.FirstName != null)
                    cmd.Parameters.AddWithValue("@firstname", filter.FirstName);

                if (filter.LastName != null)
                    cmd.Parameters.AddWithValue("@lastname", filter.LastName);

                if (filter.Status != null)
                    cmd.Parameters.AddWithValue("@status", filter.Status);

            },
            delegate (SqlDataReader reader)
            {

                while (reader.Read())
                {

                    userList.Add(new UserModel()
                    {
                        Id = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Status = reader.GetInt32(4)
                    });

                }
            });

            return userList;


        }


        public UserModel GetByUserName(string userName) {

            UserModel user = null;

            DatabaseExecuter.ExecuteQuery("SP_User_SelectByUserName", delegate (SqlCommand cmd)
            {

                if (userName != null)
                    cmd.Parameters.AddWithValue("@username", userName);

            },
            delegate (SqlDataReader reader)
            {

                if (reader.Read())
                {

                    user = new UserModel()
                    {
                        Id = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Status = reader.GetInt32(4)
                    };

                }
            });


            return user;


        }



        public UserModel Login(LoginModel userModel)
        {

            User user = null;

            DatabaseExecuter.ExecuteQuery("SP_User_Login", delegate (SqlCommand cmd)
            {

                if (userModel != null)
                    cmd.Parameters.AddWithValue("@username", userModel.UserName);

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
                        Status = reader.GetInt32(5)
                    };

                }
            });



            if (user != null && passwordHasher.VerifyHashedPassword(user.Password, userModel.Password))
            {
                return new UserModel() { Id = user.Id , UserName = user.UserName , FirstName = user.FirstName , LastName = user.LastName , Status = user.Status };
            }
            else
                return null;

        }



        public int GetStatusById(Guid id)
        {

            int status = 255;
            DatabaseExecuter.ExecuteQuery("SP_User_SelectStatusById", delegate (SqlCommand cmd)

            {

                if (id != null)
                    cmd.Parameters.AddWithValue("@Id", id);


            }, delegate (SqlDataReader reader)

            {
                if (reader.Read())
                {

                    status = reader.GetInt32(0);

                }

            });

            return status;




        }
    }
}
