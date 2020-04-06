using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using T2Access.DAL.Helper;
using T2Access.Models;
using T2Access.Models.Dtos;
using T2Access.Security;

namespace T2Access.DAL
{
    public class GateManager : IGateManager
    {
        private readonly IPasswordHasher passwordHasher = new PasswordHasher();



        #region CRUD
        public Gate Create(Gate gate)
        {

            return DatabaseExecuter.ExecuteNonQuery("SP_Gate_Insert", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_username", gate.UserName);
                cmd.Parameters.AddWithValue("_password", passwordHasher.HashPassword(gate.Password));
                cmd.Parameters.AddWithValue("_nameAr", gate.NameAr);
                cmd.Parameters.AddWithValue("_nameEn", gate.NameEn);
            }) > 0 ? gate : null;
        }

        public void Update(Gate model)
        {
            if (model.Id == null)
            {
                throw new ArgumentNullException(nameof(model.Id));
            }

            DatabaseExecuter.ExecuteNonQuery("SP_Gate_Update", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_id", model.Id);

                cmd.Parameters.AddWithValue("_username", model.UserName);

                cmd.Parameters.AddWithValue("_nameAr", model.NameAr);

                cmd.Parameters.AddWithValue("_nameEn", model.NameEn);

                cmd.Parameters.AddWithValue("_status", model.Status);

            });
        }

        public void Delete(Gate gate)
        {

            var userGateManager = ManagerFactory.GetUserGateManager(Variables.DatabaseProvider);
            // Remove all corresponding recorders for the Gate in UserGate Table  
            userGateManager.Delete(new UserGate() { GateId = gate.Id });


            DatabaseExecuter.ExecuteNonQuery("SP_Gate_Delete", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_id", gate.Id);

            });

        }


        #endregion


        //==============================================================================


        public IEnumerable<Gate> GetWithFilter(Gate filter)
        {
            IList<Gate> gateList = new List<Gate>();


            DatabaseExecuter.ExecuteQuery("SP_Gate_SelectWithFilter", delegate (SqlCommand cmd)
                     {
                         cmd.Parameters.AddWithValue("_username", filter.UserName);

                         cmd.Parameters.AddWithValue("_nameAr", filter.NameAr);

                         cmd.Parameters.AddWithValue("_nameEn", filter.NameEn);

                         cmd.Parameters.AddWithValue("_status", filter.Status);


                     }, delegate (SqlDataReader reader)
                     {
                         while (reader.Read())
                         {
                             gateList.Add(
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


            return gateList.AsEnumerable<Gate>();

        }

        public IEnumerable<CheckedGateDto> GetCheckedByUserId(Guid userId)
        {
            IList<CheckedGateDto> checkedGateList = new List<CheckedGateDto>();


            DatabaseExecuter.ExecuteQuery("SP_Gate_SelectCheckedByUserId", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_userId", userId);

            }, delegate (SqlDataReader reader)
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


        public Gate GetByUserName(string username)
        {
            Gate gate = null;
            DatabaseExecuter.ExecuteQuery("SP_Gate_SelectByUserName", delegate (SqlCommand cmd)

            {
                cmd.Parameters.AddWithValue("_username", username);

            }, delegate (SqlDataReader reader)

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

        public Gate Login(IAuthModel Gate)
        {


            Gate gate = null;
            DatabaseExecuter.ExecuteQuery("SP_Gate_Login", delegate (SqlCommand cmd)

            {

                cmd.Parameters.AddWithValue("_username", Gate.UserName);


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


            return gate != null && passwordHasher.VerifyHashedPassword(gate.Password, Gate.Password)
                ? new Gate() { Id = gate.Id, UserName = gate.UserName, NameAr = gate.NameAr, NameEn = gate.NameEn, Status = gate.Status }
                : null;
        }

        public void ResetPassword(IAuthModel model)
        {
            if (model.Id == null)
            {
                throw new ArgumentNullException(nameof(model.Id));
            }

            DatabaseExecuter.ExecuteNonQuery("SP_Gate_ResetPassword", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("_id", model.Id);

                cmd.Parameters.AddWithValue("_password", model.Password != null ? passwordHasher.HashPassword(model.Password) : "");


            });
        }

    }
}
