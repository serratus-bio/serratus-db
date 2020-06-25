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
        public DbSet<CommentLine> CommentLines { get; set; }
        public DbSet<FamilySection> FamilySections{ get; set; }
        public DbSet<AccessionSection> AccessionSections { get; set; }
        public DbSet<FastaSection> FastaSections { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("host=serratus-test-two.csa1zyyc79kj.us-east-2.rds.amazonaws.com;username=postgres;password=bandit12");
    }
}
