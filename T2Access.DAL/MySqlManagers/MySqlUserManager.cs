using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

using T2Access.DAL.Helper;
using T2Access.Models;
using T2Access.Security;


namespace T2Access.DAL
{

    public class MySqlUserManager : IUserManager
    {
        private IPasswordHasher passwordHasher = new PasswordHasher();
        IUserGateManager userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);





        #region CRUD

        public bool Create(UserSignUpModel user)
        {
            Guid id = Guid.Empty;

            DatabaseExecuter.MySqlExecuteQuery("SP_User_Insert", delegate (MySqlCommand cmd)
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

            if (id == Guid.Empty)
                return false;

            if (string.IsNullOrEmpty(user.GateList))
                return true;

            Guid gateId;
            var gatelist = user.GateList.Split(',');
            foreach (string gate in gatelist)
            {
                if (Guid.TryParse(gate, out gateId))
                    userGateManager.Create(new UserGateModel() { UserId = id, GateId = gateId });
            }

            return true;

        }

        public bool Update(UserUpdateModel user)
        {
            if (user.Id == null)
                return false;

            if (!(DatabaseExecuter.MySqlExecuteNonQuery("SP_User_Update", delegate (MySqlCommand cmd)
           {
               cmd.Parameters.AddWithValue("_id", user.Id);

               cmd.Parameters.AddWithValue("_username", ""); // Can't update username /*user.UserName != null ? user.UserName : "" */ 

               cmd.Parameters.AddWithValue("_firstname", user.FirstName != null ? user.FirstName : "");

               cmd.Parameters.AddWithValue("_lastname", user.LastName != null ? user.LastName : "");

               cmd.Parameters.AddWithValue("_status", user.Status != null ? user.Status : -1);

           }) > 0))

                return false;



            //if (string.IsNullOrEmpty(user.GateList) )
            //    return true;





            //Clear previous records
            userGateManager.Delete(new UserGateModel() { UserId = user.Id });


            if (string.IsNullOrEmpty(user.GateList))
                return true;
            //Create new records
            Guid gateId;
            var gatelist = user.GateList.Split(',');
            foreach (string gate in gatelist)
            {
                if (Guid.TryParse(gate, out gateId))
                    userGateManager.Create(new UserGateModel() { UserId = user.Id, GateId = gateId });
            }


            return true;

        }

        public bool Delete(Guid id)
        {
            userGateManager.Delete(new UserGateModel() { UserId = id });

            return DatabaseExecuter.MySqlExecuteNonQuery("SP_User_Delete", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_id", id);

            }) > 0 ? true : false;

        }

        #endregion




        public ResponseFilteredUserList GetWithFilter(UserFilterModel filter)
        {
            List<UserModel> userList = new List<UserModel>();


            DatabaseExecuter.MySqlExecuteQuery("SP_User_SelectWithFilter", delegate (MySqlCommand cmd)
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


            var _totalSize = userList.Count; 

            //paging
            if (filter.Skip != null && filter.PageSize != null)
                userList = userList.Skip((int)filter.Skip).Take((int)filter.PageSize).ToList<UserModel>();




            return new ResponseFilteredUserList() { ResponseList = userList, totalEntities = _totalSize }; ;


        }


        public UserModel GetByUserName(string userName)
        {

            UserModel user = null;

            DatabaseExecuter.MySqlExecuteQuery("SP_User_SelectByUserName", delegate (MySqlCommand cmd)
            {

                cmd.Parameters.AddWithValue("_username", userName != null ? userName : "");

            },
            delegate (MySqlDataReader reader)
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


        public UserModel GetById(Guid usedId)
        {

            UserModel user = null;

            DatabaseExecuter.MySqlExecuteQuery("SP_User_SelectById", delegate (MySqlCommand cmd)
            {

                cmd.Parameters.AddWithValue("_id", usedId != null ? usedId : Guid.Empty);

            },
            delegate (MySqlDataReader reader)
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


            if (user == null)
                return null;


            //user.GateList = userGateManager.GetByUserId(user.Id);


            return user;


        }




        public UserModel Login(LoginModel userModel)
        {

            User user = null;

            DatabaseExecuter.MySqlExecuteQuery("SP_User_Login", delegate (MySqlCommand cmd)
            {

                cmd.Parameters.AddWithValue("_username", userModel != null ? userModel.UserName : "");

            },
            delegate (MySqlDataReader reader)
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
                return new UserModel() { Id = user.Id, UserName = user.UserName, FirstName = user.FirstName, LastName = user.LastName, Status = user.Status };
            }
            else
                return null;

        }

        public bool ResetPassword(ResetPasswordModel model)
        {
            if (model.Id == null)
                return false;

            return DatabaseExecuter.MySqlExecuteNonQuery("SP_User_ResetPassword", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_id", model.Id);

                cmd.Parameters.AddWithValue("_password", model.Password != null ? passwordHasher.HashPassword(model.Password) : "");


            }) > 0 ? true : false;
        }


    }
}
