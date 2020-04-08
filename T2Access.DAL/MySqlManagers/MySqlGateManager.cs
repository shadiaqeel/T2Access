using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using T2Access.DAL.Helper;
using T2Access.Models;
using T2Access.Models.Dtos;
using T2Access.Security;

namespace T2Access.DAL
{
    public class MySqlGateManager : IGateManager
    {
        private readonly IPasswordHasher passwordHasher = new PasswordHasher();



        #region CRUD
        public async Task<Gate> CreateAsync(Gate gate)
        {

            return await DatabaseExecuter.MySqlExecuteNonQueryAsync("SP_Gate_Insert", delegate (MySqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_username", gate.UserName);
                cmd.Parameters.AddWithValue("_password", passwordHasher.HashPassword(gate.Password));
                cmd.Parameters.AddWithValue("_nameAr", gate.NameAr);
                cmd.Parameters.AddWithValue("_nameEn", gate.NameEn);
            }) > 0 ? gate : null;
        }

        public async Task UpdateAsync(Gate model)
        {
            if (model.Id == null)
            {
                throw new ArgumentNullException(nameof(model.Id));
            }

            await DatabaseExecuter.MySqlExecuteNonQueryAsync("SP_Gate_Update", delegate (MySqlCommand cmd)
             {
                 cmd.Parameters.AddWithValue("_id", model.Id);

                 cmd.Parameters.AddWithValue("_username", model.UserName != null ? model.UserName : "");

                 cmd.Parameters.AddWithValue("_nameAr", model.NameAr != null ? model.NameAr : "");

                 cmd.Parameters.AddWithValue("_nameEn", model.NameEn != null ? model.NameEn : "");

                 cmd.Parameters.AddWithValue("_status", model.Status != null ? model.Status : -1);

             });
        }

        public async Task DeleteAsync(Gate gate)
        {

            var userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);
            // Remove all corresponding recorders for the Gate in UserGate Table  
            userGateManager.DeleteAsync(new UserGate() { GateId = gate.Id });


            await DatabaseExecuter.MySqlExecuteNonQueryAsync("SP_Gate_Delete", delegate (MySqlCommand cmd)
             {
                 cmd.Parameters.AddWithValue("_id", gate.Id);

             });

        }


        #endregion


        //==============================================================================


        public async Task<IEnumerable<Gate>> GetWithFilterAsync(Gate filter)
        {
            IList<Gate> AddedGateList = new List<Gate>();


            await DatabaseExecuter.MySqlExecuteQueryAsync("SP_Gate_SelectWithFilter", delegate (MySqlCommand cmd)
                      {
                          cmd.Parameters.AddWithValue("_username", filter.UserName != null ? filter.UserName : "");

                          cmd.Parameters.AddWithValue("_nameAr", filter.NameAr != null ? filter.NameAr : "");

                          cmd.Parameters.AddWithValue("_nameEn", filter.NameEn != null ? filter.NameEn : "");

                          cmd.Parameters.AddWithValue("_status", filter.Status != null ? filter.Status : -1);


                      }, delegate (MySqlDataReader reader)
                      {
                          while (reader.Read())
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


            await DatabaseExecuter.MySqlExecuteQueryAsync("SP_Gate_SelectCheckedByUserId", delegate (MySqlCommand cmd)
             {
                 cmd.Parameters.AddWithValue("_userId", userId);

             }, delegate (MySqlDataReader reader)
             {
                 while (reader.Read())
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
            await DatabaseExecuter.MySqlExecuteQueryAsync("SP_Gate_SelectByUserName", delegate (MySqlCommand cmd)

             {
                 cmd.Parameters.AddWithValue("_username", username != null ? username : "");

             }, delegate (MySqlDataReader reader)

             {


                 if (reader.Read())
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

        public async Task<Gate> LoginAsync(IAuthModel Gate)
        {


            Gate gate = null;
            await DatabaseExecuter.MySqlExecuteQueryAsync("SP_Gate_Login", delegate (MySqlCommand cmd)

             {

                 cmd.Parameters.AddWithValue("_username", Gate != null ? Gate.UserName : "");


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


            return gate != null && passwordHasher.VerifyHashedPassword(gate.Password, Gate.Password)
                ? new Gate() { Id = gate.Id, UserName = gate.UserName, NameAr = gate.NameAr, NameEn = gate.NameEn, Status = gate.Status }
                : null;
        }

        public async Task ResetPasswordAsync(IAuthModel model)
        {
            if (model.Id == null)
            {
                throw new ArgumentNullException(nameof(model.Id));
            }

            await DatabaseExecuter.MySqlExecuteNonQueryAsync("SP_Gate_ResetPassword", delegate (MySqlCommand cmd)
             {
                 cmd.Parameters.AddWithValue("_id", model.Id);

                 cmd.Parameters.AddWithValue("_password", model.Password != null ? passwordHasher.HashPassword(model.Password) : "");


             });
        }

    }
}
