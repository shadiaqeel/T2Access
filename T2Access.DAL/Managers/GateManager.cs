﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using T2Access.Models;
using T2Access.Security;

namespace T2Access.DAL
{
    public class GateManager : IGateManager
    {
        private IPasswordHasher passwordHasher = new PasswordHasher();


        public bool  Insert(GateSignUpModel gateModel)
        {

            return DatabaseExecuter.ExecuteNonQuery("SP_Gate_Insert", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@username", gateModel.UserName);
                cmd.Parameters.AddWithValue("@password", passwordHasher.HashPassword(gateModel.Password));
                cmd.Parameters.AddWithValue("@nameAr", gateModel.NameAr);
                cmd.Parameters.AddWithValue("@nameEn", gateModel.NameEn);
            }) > 0 ? true :  false ;
        }





        public List<GateModel> GetWithFilter(GateModel gateModel)
        {
            List<GateModel> gateList = new List<GateModel>();


            DatabaseExecuter.ExecuteQuery("SP_Gate_SelectWithFilter", delegate (SqlCommand cmd)

                     {

                         if (gateModel.UserName != null)
                             cmd.Parameters.AddWithValue("@username", gateModel.UserName);

                         if (gateModel.NameAr != null)
                             cmd.Parameters.AddWithValue("@nameAr", gateModel.NameAr);

                         if (gateModel.NameEn != null)
                             cmd.Parameters.AddWithValue("@nameEn", gateModel.NameEn);

                         if (gateModel.Status != null)
                             cmd.Parameters.AddWithValue("@status", gateModel.Status);


                     }, delegate (SqlDataReader reader)

                     {


                         while (reader.Read())
                         {


                             gateList.Add(
                                            new GateModel()
                                            {
                                                Id = reader.GetGuid(0),
                                                UserName = reader.GetString(1),
                                                NameAr = reader.GetString(3),
                                                NameEn = reader.GetString(4),
                                                Status = reader.GetInt32(6)
                                            }
                                            );
                         }

                     });




            return gateList;

        }



        public GateModel GetByUserName(string username)
        {
            GateModel gate = null;
            DatabaseExecuter.ExecuteQuery("SP_Gate_SelectByUserName", delegate (SqlCommand cmd)

            {

                if (username != null)
                    cmd.Parameters.AddWithValue("@username", username);


            }, delegate (SqlDataReader reader)

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
            DatabaseExecuter.ExecuteQuery("SP_Gate_Login", delegate (SqlCommand cmd)

            {

                if (gateModel != null)
                    cmd.Parameters.AddWithValue("@username", gateModel.UserName);


            }, delegate (SqlDataReader reader)

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



        public int GetStatusById(Guid id)
        {

            int status = 255;
            DatabaseExecuter.ExecuteQuery("SP_Gate_SelectStatusById", delegate (SqlCommand cmd)

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
