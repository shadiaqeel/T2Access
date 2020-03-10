using System.Collections.Generic;
using System.Data.SqlClient;
using T2Access.Models;
using T2Access.Security;

namespace T2Access.DAL
{
    public class GateManager : IGateManager
    {
        private IPasswordHasher passwordHasher = new PasswordHasher();


        public bool  Insert(Gate gateModel)
        {

            return DatabaseExecuter.ExecuteNonQuery("SP_Gate_Insert", delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@username", gateModel.UserName);
                cmd.Parameters.AddWithValue("@password", passwordHasher.HashPassword(gateModel.Password));
                cmd.Parameters.AddWithValue("@nameAr", gateModel.NameAr);
                cmd.Parameters.AddWithValue("@nameEn", gateModel.NameEn);
            }) > 0 ? true :  false ;
        }





        public List<Gate> GetWithFilter(Gate gateModel)
        {
            List<Gate> gateList = new List<Gate>();


            DatabaseExecuter.ExecuteQuery("SP_Gate_SelectWithFilter", delegate (SqlCommand cmd)

                     {

                         if (gateModel.UserName != null)
                             cmd.Parameters.AddWithValue("@username", gateModel.UserName);

                         if (gateModel.NameAr != null)
                             cmd.Parameters.AddWithValue("@nameAr", gateModel.NameAr);

                         if (gateModel.NameEn != null)
                             cmd.Parameters.AddWithValue("@nameEn", gateModel.NameEn);

                         if (gateModel.Status != null)
                             cmd.Parameters.AddWithValue("@status", gateModel.Status);


                     }, delegate (SqlDataReader reader)

                     {


                         while (reader.Read())
                         {


                             gateList.Add(
                                            new Gate
                                            {
                                                Id = reader.GetGuid(0),
                                                UserName = reader.GetString(1),
                                                Password = reader.GetString(2),
                                                NameAr = reader.GetString(3),
                                                NameEn = reader.GetString(4),
                                                CreatedDate = reader.GetDateTime(5),
                                                Status = reader.GetInt32(6)
                                            }
                                            );
                         }

                     });




            return gateList;

        }



        public Gate GetByUserName(string username)
        {
            Gate gate = null;
            DatabaseExecuter.ExecuteQuery("SP_Gate_SelectByUserName", delegate (SqlCommand cmd)

            {

                if (username != null)
                    cmd.Parameters.AddWithValue("@username", username);


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
                        CreatedDate = reader.GetDateTime(5),
                        Status = reader.GetInt32(6)
                    };

                }

            });

            return gate;



        }



        public Gate Login(LoginModel gateModel)
        {


            Gate gate = GetByUserName(gateModel.UserName);


            if (gate != null && passwordHasher.VerifyHashedPassword(gate.Password, gateModel.Password))
               {
                gate.Password = "";
                return gate;
               }
            else
                return null;




        }
    }
}
