using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using T2Access.Models;
using T2Access.Security;

namespace T2Access.DAL
{
    public class MySqlGateManager : IGateManager
    {
        private IPasswordHasher passwordHasher = new PasswordHasher();



        #region CRUD
        public bool Create(GateSignUpModel gateModel)
        {

            return DatabaseExecuter.MySqlExecuteNonQuery("SP_Gate_Insert", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_username", gateModel.UserName);
                cmd.Parameters.AddWithValue("_password", passwordHasher.HashPassword(gateModel.Password));
                cmd.Parameters.AddWithValue("_nameAr", gateModel.NameAr);
                cmd.Parameters.AddWithValue("_nameEn", gateModel.NameEn);
            }) > 0 ? true : false;
        }

        public bool Update(GateModel model)
        {
            if (model.Id == null)
                return false;

            return DatabaseExecuter.MySqlExecuteNonQuery("SP_Gate_Update", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_id", model.Id);

                cmd.Parameters.AddWithValue("_username", model.UserName != null ? model.UserName : "");

                cmd.Parameters.AddWithValue("_nameAr", model.NameAr != null ? model.NameAr : "");

                cmd.Parameters.AddWithValue("_nameEn", model.NameEn != null ? model.NameEn : "");

                cmd.Parameters.AddWithValue("_status", model.Status != null ? model.Status : -1);

            }) > 0 ? true : false;
        }

        public bool Delete(Guid id)
        {

            return DatabaseExecuter.MySqlExecuteNonQuery("SP_Gate_Delete", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_id", id);

            }) > 0 ? true : false;

        } 


        #endregion





        public List<GateModel> GetWithFilter(GateFilterModel filter)
        {
            List<GateModel> gateList = new List<GateModel>();


            DatabaseExecuter.MySqlExecuteQuery("SP_Gate_SelectWithFilter", delegate (MySqlCommand cmd)
                     {
                         cmd.Parameters.AddWithValue("_username", filter.UserName != null ? filter.UserName : "");

                         cmd.Parameters.AddWithValue("_nameAr", filter.NameAr != null ? filter.NameAr : "");

                         cmd.Parameters.AddWithValue("_nameEn", filter.NameEn != null ? filter.NameEn : "");

                         cmd.Parameters.AddWithValue("_status", filter.Status != null ? filter.Status : -1);


                     }, delegate (MySqlDataReader reader)
                     {
                         while (reader.Read())
                         {
                             gateList.Add(
                                        new GateModel()
                                        {
                                            Id = reader.GetGuid(0),
                                            UserName = reader.GetString(1),
                                            NameAr = reader.GetString(2),
                                            NameEn = reader.GetString(3),
                                            Status = reader.GetInt32(4)
                                        }
                                         );
                         }
                     });

            return gateList;
        }


        public GateModel GetByUserName(string username)
        {
            GateModel gate = null;
            DatabaseExecuter.MySqlExecuteQuery("SP_Gate_SelectByUserName", delegate (MySqlCommand cmd)

            {
                cmd.Parameters.AddWithValue("_username", username != null ? username : "");

            }, delegate (MySqlDataReader reader)

            {


                if (reader.Read())
                {

                    gate = new GateModel()
                    {
                        Id = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        NameAr = reader.GetString(2),
                        NameEn = reader.GetString(3),
                        Status = reader.GetInt32(4)
                    };

                }

            });

            return gate;



        }

        public GateModel Login(LoginModel gateModel)
        {


            Gate gate = null;
            DatabaseExecuter.MySqlExecuteQuery("SP_Gate_Login", delegate (MySqlCommand cmd)

            {

                cmd.Parameters.AddWithValue("_username", gateModel != null ? gateModel.UserName : "");


            }, delegate (MySqlDataReader reader)

            {


                if (reader.Read())
                {

                    gate = new Gate()
                    {
                        Id = reader.GetGuid(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        NameAr = reader.GetString(3),
                        NameEn = reader.GetString(4),
                        Status = reader.GetInt32(5)
                    };

                }

            });


            if (gate != null && passwordHasher.VerifyHashedPassword(gate.Password, gateModel.Password))
            {
                return new GateModel() { Id = gate.Id, UserName = gate.UserName, NameAr = gate.NameAr, NameEn = gate.NameEn, Status = gate.Status };
            }
            else
                return null;




        }

        public bool ResetPassword(ResetPasswordModel model)
        {
            if (model.Id == null)
                return false;

            return DatabaseExecuter.MySqlExecuteNonQuery("SP_Gate_ResetPassword", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_id", model.Id);

                cmd.Parameters.AddWithValue("_password", model.Password != null ? passwordHasher.HashPassword(model.Password) : "");


            }) > 0 ? true : false;
        }

    }
}
