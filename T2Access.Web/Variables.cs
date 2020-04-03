using System.Configuration;

namespace T2Access
{
    internal static class Variables
    {


        internal static string ServerBaseAddress => ConfigurationManager.AppSettings.Get("ServerBaseAddress");

    }
}