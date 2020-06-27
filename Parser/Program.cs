using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SerratusTest.ORM;
using System;
using System.IO;

namespace ParserNs
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            var tokenConfig = new TokenConfig();
            ConfigurationBinder.Bind(configuration.GetSection("Tokens"), tokenConfig);
            var parser = new Parser(tokenConfig.AccessToken, tokenConfig.SecretToken);
            parser.GetBucketsFromS3();
        }
    }
}
