using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration; 


namespace T2Access.DAL
{
    public static class Variables
    {
        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["T2AccessConnectionString"].ConnectionString; }   }
        public static string MYSQLConnectionString { get { return ConfigurationManager.ConnectionStrings["T2AccessMySQL"].ConnectionString; }   }

        public static  string DatabaseProvider   { get{ return ConfigurationManager.AppSettings.Get("DatabaseProvider"); } }


    }
}
