﻿using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using T2Access.DAL.Helper;
using T2Access.Models;
using T2Access.Security;

namespace T2Access.DAL
{
    public class MySqlGateManager : IGateManager
    {
        private IPasswordHasher passwordHasher = new PasswordHasher();



        #region CRUD
        public Gate Create(Gate gate)
        {

            return DatabaseExecuter.MySqlExecuteNonQuery("SP_Gate_Insert", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_username", gate.UserName);
                cmd.Parameters.AddWithValue("_password", passwordHasher.HashPassword(gate.Password));
                cmd.Parameters.AddWithValue("_nameAr", gate.NameAr);
                cmd.Parameters.AddWithValue("_nameEn", gate.NameEn);
            }) > 0 ? gate : null;
        }



        public bool Update(Gate model)
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

        public bool Delete(Gate gate)
        {

            IUserGateManager userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);
            userGateManager.Delete(new UserGate() { GateId = gate.Id });


            return DatabaseExecuter.MySqlExecuteNonQuery("SP_Gate_Delete", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_id", gate.Id);

            }) > 0 ? true : false;

        } 


        #endregion




         
        public IList<Gate> GetWithFilter(Gate filter)
        {
            List<Gate> gateList = new List<Gate>();


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
                                        new Gate()
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

        public IList<CheckedGateModel> GetCheckedByUserId(Guid userId)
        {
            IList<CheckedGateModel> checkedGateList = new List<CheckedGateModel>();


            DatabaseExecuter.MySqlExecuteQuery("SP_Gate_SelectCheckedByUserId", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userId);




            }, delegate (MySqlDataReader reader)
            {
                while (reader.Read())
                {
                    checkedGateList.Add(
                               new CheckedGateModel()
                               {
                                   Checked = reader.GetBoolean(0),
                                   Id = reader.GetGuid(1),
                                   UserName = reader.GetString(2),
                                   NameAr = reader.GetString(3),
                                   NameEn = reader.GetString(4),
                                   Status = reader.GetInt32(5)
                               }
                                );
                }
            });

            return checkedGateList;
        }


        public Gate GetByUserName(string username)
        {
            Gate gate = null;
            DatabaseExecuter.MySqlExecuteQuery("SP_Gate_SelectByUserName", delegate (MySqlCommand cmd)

            {
                cmd.Parameters.AddWithValue("_username", username != null ? username : "");

            }, delegate (MySqlDataReader reader)

            {


                if (reader.Read())
                {

                    gate = new Gate()
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

        public Gate Login(IAuthModel Gate)
        {


            Gate gate = null;
            DatabaseExecuter.MySqlExecuteQuery("SP_Gate_Login", delegate (MySqlCommand cmd)

            {

                cmd.Parameters.AddWithValue("_username", Gate != null ? Gate.UserName : "");


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


            if (gate != null && passwordHasher.VerifyHashedPassword(gate.Password, Gate.Password))
            {
                return new Gate() { Id = gate.Id, UserName = gate.UserName, NameAr = gate.NameAr, NameEn = gate.NameEn, Status = gate.Status };
            }
            else
                return null;




        }

        public bool ResetPassword(IAuthModel model)
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
