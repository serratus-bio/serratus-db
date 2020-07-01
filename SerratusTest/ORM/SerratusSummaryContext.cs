﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SerratusTest.Domain.Model;

namespace SerratusTest.ORM
{
    public class SerratusSummaryContext : DbContext
    {
        public DbSet<Run> Runs { get; set; }
        public DbSet<FamilySection> FamilySections{ get; set; }
        public DbSet<AccessionSection> AccessionSections { get; set; }
        public DbSet<FastaSection> FastaSections { get; set; }
        public object Configuration { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("host=localhost;database=SerratusTest;username=postgres;password=bandit");
    }
}
