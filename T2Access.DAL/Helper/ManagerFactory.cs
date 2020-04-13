using System;

namespace T2Access.DAL.Helper
{
    public static class ManagerFactory
    {

        public static IUserManager GetUserManager(string provider = "")
        {
            if (string.IsNullOrEmpty(provider))
            {
                provider = Variables.DatabaseProvider;
            }

            switch (provider.ToUpper())
            {

                case "MSSQL":
                // return new UserManager();
                case "MYSQL":
                    return new UserManager();

                default:
                    throw new Exception("Provider is not supported ");

            }
        }

        public static IGateManager GetGateManager(string provider = "")
        {
            if (string.IsNullOrEmpty(provider))
            {
                provider = Variables.DatabaseProvider;
            }

            switch (provider.ToUpper())
            {

                case "MSSQL":
                //return new GateManager();
                case "MYSQL":
                    return new GateManager();

                default:
                    throw new Exception("Provider is not supported ");

            }
        }

        public static IUserGateManager GetUserGateManager(string provider = "")
        {
            if (string.IsNullOrEmpty(provider))
            {
                provider = Variables.DatabaseProvider;
            }

            switch (provider.ToUpper())
            {

                case "MSSQL":
                //return new UserGateManager();
                case "MYSQL":
                    return new UserGateManager();

                default:
                    throw new Exception("Provider is not supported ");

            }
        }

        public static ITransactionManager GetTransactionManager(string provider = "")
        {

            if (string.IsNullOrEmpty(provider))
            {
                provider = Variables.DatabaseProvider;
            }

            switch (provider.ToUpper())
            {

                case "MSSQL":
                //  return new TransactionManager();
                case "MYSQL":
                    return new TransactionManager();

                default:
                    throw new Exception("Provider is not supported ");

            }
        }

    }
}
