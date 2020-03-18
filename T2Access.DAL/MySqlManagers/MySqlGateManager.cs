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




        public bool Insert(GateSignUpModel gateModel)
        {

            return DatabaseExecuter.MySqlExecuteNonQuery("SP_Gate_Insert", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_username", gateModel.UserName);
                cmd.Parameters.AddWithValue("_password", passwordHasher.HashPassword(gateModel.Password));
                cmd.Parameters.AddWithValue("_nameAr", gateModel.NameAr);
                cmd.Parameters.AddWithValue("_nameEn", gateModel.NameEn);
            }) > 0 ? true : false;
        }


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

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
