﻿using System;
using MySql.Data.MySqlClient;
using T2Access.Models;

namespace T2Access.DAL
{
    public class MySqlUserGateManager : IUserGateManager
    {
        public bool Insert(UserGateModel userGate)
        {



            return DatabaseExecuter.MySqlExecuteNonQuery("SP_UserGate_Insert", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userGate.UserId);
                cmd.Parameters.AddWithValue("_gateId", userGate.GateId);

            }) > 0 ? true : false;

        }

        public bool Delete(UserGateModel userGate)
        {



            return DatabaseExecuter.MySqlExecuteNonQuery("SP_UserGate_Delete", delegate (MySqlCommand cmd)
           {
               cmd.Parameters.AddWithValue("_userId", userGate.UserId);
               cmd.Parameters.AddWithValue("_gateId", userGate.GateId);

           }) > 0 ? true : false;
        }

        public bool CheckIfExist(UserGateModel userGate)
        {
            UserGateModel _userGate = null;
            int userStatus = 255;
            int gateStatus = 255;



             DatabaseExecuter.MySqlExecuteQuery("SP_CheckIfValid", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userGate.UserId);
                cmd.Parameters.AddWithValue("_gateId", userGate.GateId);

            }, delegate (MySqlDataReader reader)
            {

                if (reader.Read())
                {

                    _userGate = new UserGateModel()
                    {
                        UserId = reader.GetGuid(0),
                        GateId = reader.GetGuid(1)

                    };

                    userStatus = reader.GetInt32(2);
                    gateStatus = reader.GetInt32(3);



                }
            });


            return (_userGate != null && userStatus == 0 && gateStatus == 0);


        }

    }
}
