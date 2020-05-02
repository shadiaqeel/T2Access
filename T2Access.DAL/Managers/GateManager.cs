using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using T2Access.DAL.Helper;
using T2Access.DAL.Options;
using T2Access.Models;
using T2Access.Models.Dtos;
using T2Access.Security;

namespace T2Access.DAL
{
    public class GateManager : IGateManager
    {
        private readonly IPasswordHasher passwordHasher = new PasswordHasher();
        private readonly IDatabaseExecuter databaseExecuter;
        private readonly IUserGateManager userGateManager;


        //========================================================================================================

        #region Constructors
        public GateManager()
        {
            this.userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);
            databaseExecuter = DbExecuterFactory.GetExecuter();
        }
        public GateManager(IOptionsMonitor<DALOptions> options , IUserGateManager userGateManager = null)
        {
            this.userGateManager = userGateManager ?? ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);
            databaseExecuter = DbExecuterFactory.GetExecuter(options);
        }

        #endregion

        //========================================================================================================

        #region CRUD
        public async Task<Gate> CreateAsync(Gate gate)
        {

            return await databaseExecuter.ExecuteNonQueryAsync("SP_Gate_Insert", delegate (DbCommand cmd)
            {
                cmd.AddParameterWithValue("Username", gate.UserName);
                cmd.AddParameterWithValue("Password", passwordHasher.HashPassword(gate.Password));
                cmd.AddParameterWithValue("NameAr", gate.NameAr);
                cmd.AddParameterWithValue("NameEn", gate.NameEn);
            }) > 0 ? gate : throw new Exception("No rows affected");


        }

        public async Task UpdateAsync(Gate model)
        {
            if (model.Id == null)
            {
                throw new ArgumentNullException(nameof(model.Id));
            }

            await databaseExecuter.ExecuteNonQueryAsync("SP_Gate_Update", delegate (DbCommand cmd)
             {
                 cmd.AddParameterWithValue("Id", model.Id);

                 //cmd.AddParameterWithValue("Username", model.UserName ?? "");

                 cmd.AddParameterWithValue("NameAr", model.NameAr ?? "");

                 cmd.AddParameterWithValue("NameEn", model.NameEn ?? "");

                 cmd.AddParameterWithValue("Status", model.Status != null ? model.Status : -1);

             });
        }

        public async Task DeleteAsync(Gate gate)
        {

            // Remove all corresponding recorders for the Gate in UserGate Table  
            await userGateManager.DeleteAsync(new UserGate() { GateId = gate.Id });


            await databaseExecuter.ExecuteNonQueryAsync("SP_Gate_Delete", delegate (DbCommand cmd)
             {
                 cmd.AddParameterWithValue("Id", gate.Id);

             });

        }


        #endregion


        //==============================================================================


        public async Task<IEnumerable<Gate>> GetWithFilterAsync(Gate filter)
        {
            IList<Gate> AddedGateList = new List<Gate>();


            await databaseExecuter.ExecuteQueryAsync("SP_Gate_SelectWithFilter", delegate (DbCommand cmd)
                      {
                          cmd.AddParameterWithValue("Username", filter.UserName ?? "");

                          cmd.AddParameterWithValue("NameAr", filter.NameAr ?? "");

                          cmd.AddParameterWithValue("NameEn", filter.NameEn ?? "");

                          cmd.AddParameterWithValue("Status", filter.Status != null ? filter.Status : -1);


                      }, async delegate (DbDataReader reader)
                      {
                          while (await reader.ReadAsync())
                          {
                              AddedGateList.Add(
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


            return AddedGateList.AsEnumerable<Gate>();

        }

        public async Task<IEnumerable<CheckedGateDto>> GetCheckedByUserIdAsync(Guid userId)
        {
            IList<CheckedGateDto> checkedGateList = new List<CheckedGateDto>();


            await databaseExecuter.ExecuteQueryAsync("SP_Gate_SelectCheckedByUserId", delegate (DbCommand cmd)
             {
                 cmd.AddParameterWithValue("UserId", userId);

             }, async delegate (DbDataReader reader)
             {
                 while (await reader.ReadAsync())
                 {
                     checkedGateList.Add(
                                new CheckedGateDto()
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

            return checkedGateList.AsEnumerable();
        }


        public async Task<Gate> GetByUserNameAsync(string username)
        {
            Gate gate = null;
            await databaseExecuter.ExecuteQueryAsync("SP_Gate_SelectByUserName", delegate (DbCommand cmd)

             {
                 cmd.AddParameterWithValue("Username", username ?? "");

             }, async delegate (DbDataReader reader)

             {


                 if (await reader.ReadAsync())
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

        public async Task<Gate> LoginAsync(IAuthModel gate)
        {


            Gate _gate = null;
            await databaseExecuter.ExecuteQueryAsync("SP_Gate_Login", delegate (DbCommand cmd)

             {

                 cmd.AddParameterWithValue("Username", gate.UserName ?? "");


             }, async delegate (DbDataReader reader)

             {


                 if (await reader.ReadAsync())
                 {

                     _gate = new Gate()
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


            return _gate != null && passwordHasher.VerifyHashedPassword(_gate.Password, gate.Password)
                ? new Gate() { Id = _gate.Id, UserName = _gate.UserName, NameAr = _gate.NameAr, NameEn = _gate.NameEn, Status = _gate.Status }
                : null;
        }

        public async Task ResetPasswordAsync(IAuthModel model)
        {
            if (model.Id == null)
            {
                throw new ArgumentNullException(nameof(model.Id));
            }

            await databaseExecuter.ExecuteNonQueryAsync("SP_Gate_ResetPassword", delegate (DbCommand cmd)
             {
                 cmd.AddParameterWithValue("Id", model.Id);

                 cmd.AddParameterWithValue("Password", model.Password != null ? passwordHasher.HashPassword(model.Password) : "");


             });
        }

    }
}
