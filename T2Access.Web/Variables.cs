using System.Configuration;

namespace T2Access
{
    internal static class Variables
    {


        internal static string ServerBaseAddress { get { return ConfigurationManager.AppSettings.Get("ServerBaseAddress"); } }

    }
}