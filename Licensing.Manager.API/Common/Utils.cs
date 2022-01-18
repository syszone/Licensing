using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Common
{
    public class Utils
    {
        public static string GetConnectionString()
        {

            //set "optional: false" to fail-fast without a file
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();

            return configuration.GetConnectionString("DefaultConnection");

        }
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}
