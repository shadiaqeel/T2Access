using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using T2Access.Models;
using System;

namespace T2Access.DAL
{
    public class GateManager : IGateManager
    {


        public bool Insert(Gate gateModel)
        {


            Action<SqlCommand> FillCmd = delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@username", gateModel.Username);
                cmd.Parameters.AddWithValue("@password", gateModel.HashedPassword);
                cmd.Parameters.AddWithValue("@nameAr", gateModel.NameAr);
                cmd.Parameters.AddWithValue("@nameEn", gateModel.NameEn);
            };

           

            return DatabaseExecuter.ExecuteNonQuery("SP_Gate_Insert",FillCmd);
        }





        public List<Gate> GetWithFilter(Gate gateModel)
        {
            List<Gate> gateList = new List<Gate>();


            Action<SqlCommand> FillCmd = delegate (SqlCommand cmd)

            {

                if (gateModel.Username != null)
                    cmd.Parameters.AddWithValue("@username", gateModel.Username);

                if (gateModel.NameAr != null)
                    cmd.Parameters.AddWithValue("@nameAr", gateModel.NameAr);

                if (gateModel.NameEn != null)
                    cmd.Parameters.AddWithValue("@nameEn", gateModel.NameEn);

                if (gateModel.Status != null)
                    cmd.Parameters.AddWithValue("@status", gateModel.Status);


            };




            Action<SqlDataReader> FillReader = delegate (SqlDataReader reader)

            {
                do
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Gate _gate = new Gate();

                            gateModel.Id = reader.GetGuid(0);
                            gateModel.Username = reader.GetString(1);
                            gateModel.HashedPassword = reader.GetString(2);
                            gateModel.NameAr = reader.GetString(3);
                            gateModel.NameEn = reader.GetString(4);
                            gateModel.CreatedDate = reader.GetDateTime(5);
                            gateModel.Status = reader.GetInt32(6);



                            gateList.Add(_gate);

                        }


                    }
                    else gateList = null;

                } while (reader.NextResult());
            };


             DatabaseExecuter.ExecuteQuery("SP_Gate_SelectWithFilter", FillCmd, FillReader);




            return gateList;

        }


    }
}
