using System;
using System.Data.SqlClient;
using T2Access.Models;

namespace T2Access.DAL
{
    public class UserGateManager : IUserGateManager
    {
        public bool Insert(UserGateModel userGate)
        {



            return DatabaseExecuter.ExecuteNonQuery("SP_UserGate_Insert", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@userId", userGate.UserId);
                cmd.Parameters.AddWithValue("@gateId", userGate.GateId);

            }) > 0 ? true : false;

        }

        public bool Delete(UserGateModel userGate)
        {

             

            return DatabaseExecuter.ExecuteNonQuery("SP_UserGate_Delete", delegate (SqlCommand cmd)
           {
               cmd.Parameters.AddWithValue("@userId", userGate.UserId);
               cmd.Parameters.AddWithValue("@gateId", userGate.GateId);

           }) > 0 ? true : false;
        }

        public bool CheckIfExist(UserGateModel userGate)
        {
            UserGateModel _userGate = null;
            int userStatus = 255;
            int gateStatus = 255;



             DatabaseExecuter.ExecuteQuery("SP_CheckIfValid", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@userId", userGate.UserId);
                cmd.Parameters.AddWithValue("@gateId", userGate.GateId);

            }, delegate (SqlDataReader reader)
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
