using System;
using Microsoft.Extensions.Options;
using T2Access.DAL.DbExecuter;
using T2Access.DAL.Options;

namespace T2Access.DAL.Helper
{
    public static class DbExecuterFactory
    {


        public static IDatabaseExecuter GetExecuter(IOptionsMonitor<DALOptions> options)
        {
            


            switch (options.CurrentValue.DatabaseProvider.ToUpper())
            {
                // TODO  : Indexing by string in DatabaseExecuter
                case "MSSQL":
                    return new SqlDatabaseExecuter(options.CurrentValue.ConnectionStrings[0].ConnectionString);
                case "MYSQL":
                    return new MySqlDatabaseExecuter(options.CurrentValue.ConnectionStrings[1].ConnectionString);

                default:
                    throw new Exception("Provider is not supported ");

            }
        }



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
