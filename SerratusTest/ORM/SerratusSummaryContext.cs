using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SerratusTest.Domain.Model;

namespace SerratusTest.ORM
{
    public class SerratusSummaryContext : DbContext
    {
        public DbSet<Run> run { get; set; }
        public DbSet<Family> family { get; set; }
        public DbSet<Sequence> sequence { get; set; }
        public object Configuration { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(Helper.GetRDSConnectionString());
    }
}
