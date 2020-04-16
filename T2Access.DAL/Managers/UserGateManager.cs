using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using T2Access.DAL.Helper;
using T2Access.DAL.Options;

namespace T2Access.DAL
{
    public class UserGateManager : IUserGateManager
    {


        private readonly IDatabaseExecuter databaseExecuter ;

        //=====================================================================================

        #region Constructors
        public UserGateManager()
        {
            databaseExecuter = DbExecuterFactory.GetExecuter();
        }
        public UserGateManager(IOptionsMonitor<DALOptions> options)
        {
            databaseExecuter = DbExecuterFactory.GetExecuter(options);
        } 
        #endregion
        //========================================================================================================



        public async Task<UserGate> CreateAsync(UserGate userGate)
        {



            return await databaseExecuter.ExecuteNonQueryAsync("SP_UserGate_Insert", delegate (DbCommand cmd)
            {
                cmd.AddParameterWithValue("UserId", userGate.UserId);
                cmd.AddParameterWithValue("GateId", userGate.GateId);

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

            await databaseExecuter.ExecuteNonQueryAsync("SP_UserGate_Delete", delegate (DbCommand cmd)
            {
                cmd.AddParameterWithValue("UserId", userGate.UserId);
                cmd.AddParameterWithValue("GateId", userGate.GateId);

            });
        }

        public Task UpdateAsync(UserGate editmodel)
        {
            throw new NotImplementedException();
        }

        //========================================================================================

        public async Task DeleteAllByUserIdAsync(Guid userId)
        {

            await databaseExecuter.ExecuteNonQueryAsync("SP_UserGate_DeleteAllByUserId", delegate (DbCommand cmd)
             {
                 cmd.AddParameterWithValue("UserId", userId);

             });

        }

        public async Task<bool> CheckIfExistAsync(UserGate userGate)
        {
            UserGate _userGate = null;
            int userStatus = 255;
            int gateStatus = 255;



            await databaseExecuter.ExecuteQueryAsync("SP_CheckIfValid", delegate (DbCommand cmd)
            {
                cmd.AddParameterWithValue("UserId", userGate.UserId);
                cmd.AddParameterWithValue("GateId", userGate.GateId);

            }, async delegate (DbDataReader reader)
            {

                if (await reader.ReadAsync())
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


            await databaseExecuter.ExecuteQueryAsync("SP_UserGate_GetByUserId", delegate (DbCommand cmd)
            {
                cmd.AddParameterWithValue("UserId", userid);

            }, async delegate (DbDataReader reader)
            {

                while (await reader.ReadAsync())
                {

                    AddedGateList.Add(reader.GetGuid(0));

                }
            });

            return AddedGateList;
        }

    }


}
