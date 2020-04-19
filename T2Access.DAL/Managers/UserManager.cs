using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using T2Access.DAL.Helper;
using T2Access.DAL.Options;
using T2Access.Models;
using T2Access.Security;

namespace T2Access.DAL
{
    public class UserManager : IUserManager
    {
        private readonly IPasswordHasher passwordHasher = new PasswordHasher();

        private readonly IDatabaseExecuter databaseExecuter;


        //============================================================================================

        #region Constructors
        public UserManager()
        {
            databaseExecuter = DbExecuterFactory.GetExecuter();
        }
        public UserManager(IOptionsMonitor<DALOptions> options)
        {
            databaseExecuter = DbExecuterFactory.GetExecuter(options);
        }
        #endregion
        //========================================================================================================


        public async Task<User> CreateAsync(User user)
        {
            Guid id = Guid.Empty;

            await databaseExecuter.ExecuteQueryAsync("SP_User_Insert", delegate (DbCommand cmd)
            {
                cmd.AddParameterWithValue("Username", user.UserName);
                cmd.AddParameterWithValue("Password", passwordHasher.HashPassword(user.Password));
                cmd.AddParameterWithValue("Firstname", user.FirstName);
                cmd.AddParameterWithValue("Lastname", user.LastName);
            }, async (DbDataReader reader) =>
            {


                if (await reader.ReadAsync())
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

            await databaseExecuter.ExecuteNonQueryAsync("SP_User_Update", delegate (DbCommand cmd)
            {

                cmd.AddParameterWithValue("Id", user.Id);

                //cmd.AddParameterWithValue("Username", ""); // Can't update username /*user.UserName != null ? user.UserName : "" */ 

                cmd.AddParameterWithValue("Firstname", user.FirstName ?? "");

                cmd.AddParameterWithValue("Lastname", user.LastName ?? "");

                cmd.AddParameterWithValue("Status", user.Status != null ? user.Status : -1);

            });
        }

        public async Task DeleteAsync(User user)
        {

            await databaseExecuter.ExecuteNonQueryAsync("SP_User_Delete", delegate (DbCommand cmd)
             {
                 cmd.AddParameterWithValue("Id", user.Id);

             });
        }


        //========================================================================================================

        public async Task<IEnumerable<User>> GetWithFilterAsync(User filter)
        {
            IList<User> userList = new List<User>();


            await databaseExecuter.ExecuteQueryAsync("SP_User_SelectWithFilter", delegate (DbCommand cmd)
             {

                 //cmd.AddParameterWithValue("_id", filter.Id != null ? filter.Id : Guid.Empty);

                 cmd.AddParameterWithValue("Username", filter.UserName ?? "");

                 cmd.AddParameterWithValue("Firstname", filter.FirstName ?? "");

                 cmd.AddParameterWithValue("Lastname", filter.LastName ?? "");

                 cmd.AddParameterWithValue("Status", filter.Status != null ? filter.Status : -1);

             },
            async delegate (DbDataReader reader)
            {

                while (await reader.ReadAsync())
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

            await databaseExecuter.ExecuteQueryAsync("SP_User_SelectByUserName", delegate (DbCommand cmd)
             {

                 cmd.AddParameterWithValue("Username", userName ?? "");

             },
            async delegate (DbDataReader reader)
            {

                if (await reader.ReadAsync())
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

            await databaseExecuter.ExecuteQueryAsync("SP_User_SelectById", delegate (DbCommand cmd)
             {

                 cmd.AddParameterWithValue("Id", usedId != null ? usedId : Guid.Empty);

             },
            async delegate (DbDataReader reader)
            {

                if (await reader.ReadAsync())
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

            await databaseExecuter.ExecuteQueryAsync("SP_User_Login", delegate (DbCommand cmd)
             {

                 cmd.AddParameterWithValue("Username", user.UserName ?? "");

             },
            async delegate (DbDataReader reader)
            {

                if (await reader.ReadAsync())
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

            await databaseExecuter.ExecuteNonQueryAsync("SP_User_ResetPassword", delegate (DbCommand cmd)
             {
                 cmd.AddParameterWithValue("Id", model.Id);

                 cmd.AddParameterWithValue("Password", model.Password != null ? passwordHasher.HashPassword(model.Password) : "");


             });
        }
    }
}
