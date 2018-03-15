using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BidSoftware.Shared
{
    public static class Configuration
    {
        public class ConfigurationKey
        {
            private readonly string val;
            private ConfigurationKey(string value)
            {
                val = value;
            }

            public string GetKey() { return val; }

            public static ConfigurationKey AdminUser = new ConfigurationKey("admin");
            public static ConfigurationKey AdminPass = new ConfigurationKey("adminPass");
            public static ConfigurationKey Server = new ConfigurationKey("server");
            public static ConfigurationKey Database = new ConfigurationKey("database");
            public static ConfigurationKey Schema = new ConfigurationKey("schema");
            public static ConfigurationKey TablePageSize = new ConfigurationKey("tablePageSize");
            public static ConfigurationKey DbUser = new ConfigurationKey("dbUser");
            public static ConfigurationKey DbPassword = new ConfigurationKey("dbPass");
        }

        public static string GetConfigValue(ConfigurationKey key)
        {
            return ConfigurationManager.AppSettings[key.GetKey()];
        }
    }
}
