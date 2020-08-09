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
using SerratusDb.Domain.Model;
using SerratusDb.Services;

namespace SerratusDb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RunParser();
        }

        public static async void RunParser()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            var tokenConfig = new TokenConfig();
            ConfigurationBinder.Bind(configuration.GetSection("Tokens"), tokenConfig);
            var parser = new Parser(tokenConfig.AccessToken, tokenConfig.SecretToken);
            //parser.GetBucketsFromS3();
            await parser.GetDataFromBucketList();
            Console.WriteLine("COMPLETE");
        }
    }
}
