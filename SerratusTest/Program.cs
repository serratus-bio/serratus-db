using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParserNs;
using SerratusTest.Services;

namespace SerratusTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RunParser();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void RunParser()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            NameValueCollection appConfig = ConfigurationManager.AppSettings;
            string access = appConfig["AccessToken"];
            string secret = appConfig["SecretToken"];
            var parser = new Parser(access, secret);
            parser.GetBucketsFromS3();
        }

    }
}
