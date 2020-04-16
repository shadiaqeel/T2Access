using System;
using System.Collections.Generic;
using System.Text;

namespace T2Access.DAL.Options
{
    public class DALOptions
    {
        public string DatabaseProvider { get; set; }
        public IList<ConnectionStringOption> ConnectionStrings { get; set; }

    }


    public  class ConnectionStringOption
    {
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

    }
}
