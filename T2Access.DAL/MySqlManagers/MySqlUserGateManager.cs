using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using T2Access.Models; 

namespace T2Access.DAL
{
    public class MySqlUserGateManager : IUserGateManager
    {
        public bool Create(UserGateModel userGate)
        {



            return DatabaseExecuter.MySqlExecuteNonQuery("SP_UserGate_Insert", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userGate.UserId);
                cmd.Parameters.AddWithValue("_gateId", userGate.GateId);

            }) > 0 ? true : false;

        }

        public bool Delete(UserGateModel userGate)
        {
            if (userGate == null || (userGate.UserId == Guid.Empty && userGate.GateId == Guid.Empty))
                return false;

            return DatabaseExecuter.MySqlExecuteNonQuery("SP_UserGate_Delete", delegate (MySqlCommand cmd)
           {
               cmd.Parameters.AddWithValue("_userId", userGate.UserId );
               cmd.Parameters.AddWithValue("_gateId", userGate.GateId );

           }) > 0 ? true : false;
        }

        public bool DeleteAllByUserId(Guid userId)
        {

            return DatabaseExecuter.MySqlExecuteNonQuery("SP_UserGate_DeleteAllByUserId", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userId);

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

        public List<Guid> GetByUserId(Guid userid)
        {

            List<Guid> gateList = new List<Guid>();


            DatabaseExecuter.MySqlExecuteQuery("SP_UserGate_GetByUserId", delegate (MySqlCommand cmd)
           {
               cmd.Parameters.AddWithValue("_userId", userid);

           }, delegate (MySqlDataReader reader)
           {

               while (reader.Read())
               {

                   gateList.Add(reader.GetGuid(0));

               }
           });

            return gateList;
        }


        public bool Update(UserGateModel editmodel)
        {
            throw new NotImplementedException();
        }
    }


}
