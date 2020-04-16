using System.Configuration;


namespace T2Access.DAL
{
    public static class Variables
    {



        public static string ConnectionString => ConfigurationManager.ConnectionStrings["T2AccessConnectionString"].ConnectionString;
        public static string MYSQLConnectionString => ConfigurationManager.ConnectionStrings["T2AccessMySQL"].ConnectionString;
        public static string DatabaseProvider => ConfigurationManager.AppSettings.Get("DatabaseProvider"); 


    }
}
