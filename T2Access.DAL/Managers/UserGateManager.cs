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
    }
}
