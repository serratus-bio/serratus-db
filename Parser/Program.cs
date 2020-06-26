using Microsoft.Extensions.DependencyInjection;
using SerratusTest.ORM;
using System;

namespace ParserNs
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();
            parser.GetBucketsFromS3();
            Console.WriteLine("help");
            //parser.ReadFile();
            //parser.ParseFile();

        }
    }
}
