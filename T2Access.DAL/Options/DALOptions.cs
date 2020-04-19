using System.Collections.Generic;

namespace T2Access.DAL.Options
{
    public class DALOptions
    {
        public string DatabaseProvider { get; set; }
        public IList<ConnectionStringOption> ConnectionStrings { get; set; }

    }


    public class ConnectionStringOption
    {
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

    }
}
