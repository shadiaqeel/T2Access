using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using T2Access.DAL.Helper;
using T2Access.Models;
using T2Access.Security;

namespace T2Access.DAL
{
    public class MySqlUserManager : IUserManager
    {
        private readonly IPasswordHasher passwordHasher = new PasswordHasher();

        private readonly IUserGateManager userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);

        //========================================================================================================


        public async Task<User> CreateAsync(User user)
        {
            Guid id = Guid.Empty;

            await DatabaseExecuter.MySqlExecuteQueryAsync("SP_User_Insert", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_username", user.UserName);
                cmd.Parameters.AddWithValue("_password", passwordHasher.HashPassword(user.Password));
                cmd.Parameters.AddWithValue("_firstname", user.FirstName);
                cmd.Parameters.AddWithValue("_lastname", user.LastName);
            }, delegate (MySqlDataReader reader)
            {

                if (reader.Read())
                {
                    id = reader.GetGuid(0);

                }
            });

            user.Id = id;
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            if (user.Id == null)
            {
                throw new ArgumentNullException(nameof(user.Id));
            }

            await DatabaseExecuter.MySqlExecuteNonQueryAsync("SP_User_Update", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_id", user.Id);

                cmd.Parameters.AddWithValue("_username", ""); // Can't update username /*user.UserName != null ? user.UserName : "" */ 

                cmd.Parameters.AddWithValue("_firstname", user.FirstName != null ? user.FirstName : "");

                cmd.Parameters.AddWithValue("_lastname", user.LastName != null ? user.LastName : "");

                cmd.Parameters.AddWithValue("_status", user.Status != null ? user.Status : -1);

            });
        }

        public async Task DeleteAsync(User user)
        {

            await DatabaseExecuter.MySqlExecuteNonQueryAsync("SP_User_Delete", delegate (MySqlCommand cmd)
             {
                 cmd.Parameters.AddWithValue("_id", user.Id);

             });
        }


        //========================================================================================================

        public async Task<IEnumerable<User>> GetWithFilterAsync(User filter)
        {
            IList<User> userList = new List<User>();


            await DatabaseExecuter.MySqlExecuteQueryAsync("SP_User_SelectWithFilter", delegate (MySqlCommand cmd)
             {

                 //cmd.Parameters.AddWithValue("_id", filter.Id != null ? filter.Id : Guid.Empty);

                 cmd.Parameters.AddWithValue("_username", filter.UserName != null ? filter.UserName : "");

                 cmd.Parameters.AddWithValue("_firstname", filter.FirstName != null ? filter.FirstName : "");

                 cmd.Parameters.AddWithValue("_lastname", filter.LastName != null ? filter.LastName : "");

                 cmd.Parameters.AddWithValue("_status", filter.Status != null ? filter.Status : -1);

             },
            delegate (MySqlDataReader reader)
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

        public async Task<User> GetByUserNameAsync(string userName)
        {

            User user = null;

            await DatabaseExecuter.MySqlExecuteQueryAsync("SP_User_SelectByUserName", delegate (MySqlCommand cmd)
             {

                 cmd.Parameters.AddWithValue("_username", userName != null ? userName : "");

             },
            delegate (MySqlDataReader reader)
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

        public async Task<User> GetByIdAsync(Guid usedId)
        {

            User user = null;

            await DatabaseExecuter.MySqlExecuteQueryAsync("SP_User_SelectById", delegate (MySqlCommand cmd)
             {

                 cmd.Parameters.AddWithValue("_id", usedId != null ? usedId : Guid.Empty);

             },
            delegate (MySqlDataReader reader)
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

        public async Task<User> LoginAsync(IAuthModel user)
        {

           User _user = null;

            await DatabaseExecuter.MySqlExecuteQueryAsync("SP_User_Login", delegate (MySqlCommand cmd)
             {

                 cmd.Parameters.AddWithValue("_username", user != null ? user.UserName : "");

             },
            delegate (MySqlDataReader reader)
            {

                if (reader.Read())
                {

                    _user = new User()
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



            return _user != null && passwordHasher.VerifyHashedPassword(_user.Password, user.Password)
                ? new User() { Id = _user.Id, UserName = _user.UserName, FirstName = _user.FirstName, LastName = _user.LastName, Status = _user.Status }
                : null;
        }

        public async Task ResetPasswordAsync(IAuthModel model)
        {
            if (model.Id == null)
            {
                throw new ArgumentNullException(nameof(model.Id));
            }

            await DatabaseExecuter.MySqlExecuteNonQueryAsync("SP_User_ResetPassword", delegate (MySqlCommand cmd)
             {
                 cmd.Parameters.AddWithValue("_id", model.Id);

                 cmd.Parameters.AddWithValue("_password", model.Password != null ? passwordHasher.HashPassword(model.Password) : "");


             });
        }
    }
}
