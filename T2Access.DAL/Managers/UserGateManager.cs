using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace T2Access.DAL
{
    public class UserGateManager : IUserGateManager
    {
        public bool Insert(Guid userId, Guid gateId)
        {

            Action<SqlCommand> FillCmd = delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@gateId", gateId);

            };


            return DatabaseExecuter.ExecuteNonQuery("SP_UserGate_Insert", FillCmd);

        }

        public bool Delete(Guid userId, Guid gateId)
        {

            Action<SqlCommand> FillCmd = delegate (SqlCommand cmd)
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@gateId", gateId);

            };

            return DatabaseExecuter.ExecuteNonQuery("SP_UserGate_Delete",FillCmd);
        }
    }
}
