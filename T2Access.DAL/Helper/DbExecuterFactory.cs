using System;

using T2Access.DAL.DbExecuter;

namespace T2Access.DAL.Helper
{
    public static class DbExecuterFactory
    {

        public static IDatabaseExecuter GetExecuter(string provider = "")
        {
            if (string.IsNullOrEmpty(provider))
            {
                provider = Variables.DatabaseProvider;
            }


            switch (provider.ToUpper())
            {

                case "MSSQL":
                    return new SqlDatabaseExecuter();
                case "MYSQL":
                    return new MySqlDatabaseExecuter();

                default:
                    throw new Exception("Provider is not supported ");

            }
        }



    }
}
