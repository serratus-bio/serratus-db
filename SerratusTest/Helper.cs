using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace SerratusTest
{
    public class Helper
    {
        private static string _connectionString;
        public static string GetRDSConnectionString()
        {
            if (_connectionString != null)
            {
                return _connectionString;
            }
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            string dbname = configuration["RDS_DB_NAME"];

            if (string.IsNullOrEmpty(dbname)) return null;

            string username = configuration["RDS_USERNAME"];
            string password = configuration["RDS_PASSWORD"];
            string hostname = configuration["RDS_HOSTNAME"];

            _connectionString = "host=" + hostname + ";database=" + dbname + ";username=" + username + ";password=" + password ;
            return _connectionString;
        }
    }
}

