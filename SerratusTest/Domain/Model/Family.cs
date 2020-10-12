using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerratusTest.Domain.Model
{
    public class Family
    {
        public int family_id { get; set; }
        public int family_line { get; set; }
        public int run_id { get; set; }
        public string sra_id { get; set; }
        public string family_name { get; set; }
        public int score { get; set; }
        public int percent_identity { get; set; }
        public string coverage_bins { get; set; }
        public int n_reads { get; set; }
        public int n_global_reads { get; set; }
        public int length { get; set; }
        public int depth { get; set; }
        public string top_genbank_id { get; set; }
        public string top_name { get; set; }
        public int top_score { get; set; }
        public int top_length { get; set; }
    }
}
