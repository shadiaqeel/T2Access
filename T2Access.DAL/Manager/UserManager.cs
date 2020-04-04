using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using T2Access.DAL.Helper;
using T2Access.Models;
using T2Access.Security;


namespace T2Access.DAL
{

    public class UserManager : IUserManager
    {
        private readonly IPasswordHasher passwordHasher = new PasswordHasher();
        private readonly IUserGateManager userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);





        #region CRUD

        public User Create(User user)
        {
            Guid id = Guid.Empty;

            DatabaseExecuter.ExecuteQuery("SP_User_Insert", delegate (SqlCommand cmd)
           {
               cmd.Parameters.AddWithValue("_username", user.UserName);
               cmd.Parameters.AddWithValue("_password", passwordHasher.HashPassword(user.Password));
               cmd.Parameters.AddWithValue("_firstname", user.FirstName);
               cmd.Parameters.AddWithValue("_lastname", user.LastName);
           }, delegate (SqlDataReader reader)
           {

               if (reader.Read())
               {
                   id = reader.GetGuid(0);

               }
           });

            user.Id = id;
            return user;

        }

        public bool Update(User user)
        {
            if (user.Id == null)
            {
                return false;
            }

            return DatabaseExecuter.ExecuteNonQuery("SP_User_Update", delegate (SqlCommand cmd)
           {
               cmd.Parameters.AddWithValue("_id", user.Id);

               cmd.Parameters.AddWithValue("_username", ""); // Can't update username /*user.UserName != null ? user.UserName : "" */ 

               cmd.Parameters.AddWithValue("_firstname", user.FirstName );

               cmd.Parameters.AddWithValue("_lastname", user.LastName);

               cmd.Parameters.AddWithValue("_status", user.Status );

           }) > 0 ? true : false;






        }

        public bool Delete(User user)
        {

            return DatabaseExecuter.ExecuteNonQuery("SP_User_Delete", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_id", user.Id);

            }) > 0 ? true : false;

        }

        #endregion

        //==============================================================================


        public IEnumerable<User> GetWithFilter(User filter)
        {
            IList<User> userList = new List<User>();


            DatabaseExecuter.ExecuteQuery("SP_User_SelectWithFilter", delegate (SqlCommand cmd)
            {

                //cmd.Parameters.AddWithValue("_id", filter.Id != null ? filter.Id : Guid.Empty);

                cmd.Parameters.AddWithValue("_username", filter.UserName);

                cmd.Parameters.AddWithValue("_firstname", filter.FirstName);

                cmd.Parameters.AddWithValue("_lastname", filter.LastName );

                cmd.Parameters.AddWithValue("_status", filter.Status );

            },
            delegate (SqlDataReader reader)
            {

                while (reader.Read())
                {

                    userList.Add(new User()
                    {
                        Id = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Status = reader.GetInt32(4)
                    });

                }
            });


            return userList.AsEnumerable();




        }


        public User GetByUserName(string userName)
        {

            User user = null;

            DatabaseExecuter.ExecuteQuery("SP_User_SelectByUserName", delegate (SqlCommand cmd)
            {

                cmd.Parameters.AddWithValue("_username", userName );

            },
            delegate (SqlDataReader reader)
            {

                if (reader.Read())
                {

                    user = new User()
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


        public User GetById(Guid usedId)
        {

            User user = null;

            DatabaseExecuter.ExecuteQuery("SP_User_SelectById", delegate (SqlCommand cmd)
            {

                cmd.Parameters.AddWithValue("_id", usedId != null ? usedId : Guid.Empty);

            },
            delegate (SqlDataReader reader)
            {

                if (reader.Read())
                {

                    user = new User()
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


        public User Login(IAuthModel User)
        {

            User user = null;

            DatabaseExecuter.ExecuteQuery("SP_User_Login", delegate (SqlCommand cmd)
            {

                cmd.Parameters.AddWithValue("_username", User );

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



            return user != null && passwordHasher.VerifyHashedPassword(user.Password, User.Password)
                ? new User() { Id = user.Id, UserName = user.UserName, FirstName = user.FirstName, LastName = user.LastName, Status = user.Status }
                : null;
        }

        public bool ResetPassword(IAuthModel model)
        {
            if (model.Id == null)
            {
                return false;
            }

            return DatabaseExecuter.ExecuteNonQuery("SP_User_ResetPassword", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_id", model.Id);

                cmd.Parameters.AddWithValue("_password", model.Password != null ? passwordHasher.HashPassword(model.Password) : "");


            }) > 0 ? true : false;
        }


    }
}
