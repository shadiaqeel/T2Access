using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2Access.DAL.Helper
{
    public static class ManagerFactory
    {

        public static IUserManager GetUserManager(string provider)
        {
            if (String.IsNullOrEmpty(provider))
                provider = Variables.DatabaseProvider;


            switch (provider.ToUpper())
            {

                case "MSSQL":
                    return new UserManager();
                case "MYSQL":
                    return new MySqlUserManager();

                default:
                    throw new Exception("Provider is not supported ");
            
            }
        }

        public static IGateManager GetGateManager(string provider)
        {
            if (String.IsNullOrEmpty(provider))
                provider = Variables.DatabaseProvider;

            switch (provider.ToUpper())
            {

                case "MSSQL":
                    return new GateManager();
                case "MYSQL":
                    return new MySqlGateManager();

                default:
                    throw new Exception("Provider is not supported ");

            }
        }

        public static IUserGateManager GetUserGateManager(string provider)
        {
            if (String.IsNullOrEmpty(provider))
                provider = Variables.DatabaseProvider;


            switch (provider.ToUpper())
            {

                case "MSSQL":
                    return new UserGateManager();
                case "MYSQL":
                    return new MySqlUserGateManager();

                default:
                    throw new Exception("Provider is not supported ");

            }
        }

        public static ITransactionManager GetTransactionManager(string provider)
        {

            if (String.IsNullOrEmpty(provider))
                provider = Variables.DatabaseProvider;


            switch (provider.ToUpper())
            {

                case "MSSQL":
                    return new TransactionManager();
                case "MYSQL":
                    return new MySqlTransactionManager();

                default:
                    throw new Exception("Provider is not supported ");

            }
        }

    }
}
