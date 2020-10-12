using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerratusTest.Domain.Model
{
    public class Sequence
    {
        public int sequence_id { get; set; }
        public int sequence_line { get; set; }
        public int run_id { get; set; }
        public int sra_id { get; set; }
        public int family_id { get; set; }
        public string genbank_id { get; set; }
        public string genbank_name { get; set; }
        public string family_name { get; set; }
        public int score { get; set; }
        public int percentage_identity { get; set; }
        public string coverage_bins { get; set; }
        public int n_reads { get; set; }
        public int n_global_reads { get; set; }
        public int length { get; set; }
        public double depth { get; set; }
    }
}
