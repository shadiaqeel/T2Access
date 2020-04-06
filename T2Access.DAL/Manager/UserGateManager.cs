using System;
using System.Collections.Generic;

using System.Data.SqlClient;

namespace T2Access.DAL
{
    public class UserGateManager : IUserGateManager
    {
        public UserGate Create(UserGate userGate)
        {



            return DatabaseExecuter.ExecuteNonQuery("SP_UserGate_Insert", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userGate.UserId);
                cmd.Parameters.AddWithValue("_gateId", userGate.GateId);

            }) > 0 ? userGate : null;

        }

        public void Delete(UserGate userGate)
        {
            if (userGate == null)
            {
                throw new ArgumentNullException(nameof(userGate));
            }

            if (userGate.UserId == Guid.Empty && userGate.GateId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userGate));
            }

            DatabaseExecuter.ExecuteNonQuery("SP_UserGate_Delete", delegate (SqlCommand cmd)
           {
               cmd.Parameters.AddWithValue("_userId", userGate.UserId);
               cmd.Parameters.AddWithValue("_gateId", userGate.GateId);

           });
        }

        public void DeleteAllByUserId(Guid userId)
        {

            DatabaseExecuter.ExecuteNonQuery("SP_UserGate_DeleteAllByUserId", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userId);

            });

        }

        public bool CheckIfExist(UserGate userGate)
        {
            UserGate _userGate = null;
            int userStatus = 255;
            int gateStatus = 255;



            DatabaseExecuter.ExecuteQuery("SP_CheckIfValid", delegate (SqlCommand cmd)
           {
               cmd.Parameters.AddWithValue("_userId", userGate.UserId);
               cmd.Parameters.AddWithValue("_gateId", userGate.GateId);

           }, delegate (SqlDataReader reader)
           {

               if (reader.Read())
               {

                   _userGate = new UserGate()
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


            DatabaseExecuter.ExecuteQuery("SP_UserGate_GetByUserId", delegate (SqlCommand cmd)
           {
               cmd.Parameters.AddWithValue("_userId", userid);

           }, delegate (SqlDataReader reader)
           {

               while (reader.Read())
               {

                   gateList.Add(reader.GetGuid(0));

               }
           });

            return gateList;
        }

        public void Update(UserGate editmodel)
        {
            throw new NotImplementedException();
        }
    }


}
