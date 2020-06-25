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
            parser.ReadFile();
            parser.ParseFile();
            using (var context = new SerratusSummaryContext())
            {
                context.CommentLines.Add(parser.CommentLine);
                context.SaveChanges();
            }
        }
    }
}
