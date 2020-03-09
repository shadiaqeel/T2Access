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



    }
}
