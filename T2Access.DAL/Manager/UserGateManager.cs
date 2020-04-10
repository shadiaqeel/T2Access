using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace T2Access.DAL
{
    public class UserGateManager : IUserGateManager
    {
        public async Task<UserGate> CreateAsync(UserGate userGate)
        {



            return await DatabaseExecuter.MySqlExecuteNonQueryAsync("SP_UserGate_Insert", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userGate.UserId);
                cmd.Parameters.AddWithValue("_gateId", userGate.GateId);

            }) > 0 ? userGate : null;

        }

        public async Task DeleteAsync(UserGate userGate)
        {
            if (userGate == null)
            {
                throw new ArgumentNullException(nameof(userGate));
            }

            if (userGate.UserId == Guid.Empty && userGate.GateId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userGate));
            }

            await DatabaseExecuter.MySqlExecuteNonQueryAsync("SP_UserGate_Delete", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userGate.UserId);
                cmd.Parameters.AddWithValue("_gateId", userGate.GateId);

            });
        }

        public Task UpdateAsync(UserGate editmodel)
        {
            throw new NotImplementedException();
        }

        //========================================================================================

        public async Task DeleteAllByUserIdAsync(Guid userId)
        {

            await DatabaseExecuter.MySqlExecuteNonQueryAsync("SP_UserGate_DeleteAllByUserId", delegate (MySqlCommand cmd)
             {
                 cmd.Parameters.AddWithValue("_userId", userId);

             });

        }

        public async Task<bool> CheckIfExistAsync(UserGate userGate)
        {
            UserGate _userGate = null;
            int userStatus = 255;
            int gateStatus = 255;



            await DatabaseExecuter.MySqlExecuteQueryAsync("SP_CheckIfValid", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userGate.UserId);
                cmd.Parameters.AddWithValue("_gateId", userGate.GateId);

            }, delegate (MySqlDataReader reader)
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

        public async Task<List<Guid>> GetByUserIdAsync(Guid userid)
        {

            List<Guid> AddedGateList = new List<Guid>();


            await DatabaseExecuter.MySqlExecuteQueryAsync("SP_UserGate_GetByUserId", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userid);

            }, delegate (MySqlDataReader reader)
            {

                while (reader.Read())
                {

                    AddedGateList.Add(reader.GetGuid(0));

                }
            });

            return AddedGateList;
        }

    }


}
