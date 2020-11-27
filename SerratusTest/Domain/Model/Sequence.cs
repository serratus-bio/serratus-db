using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SerratusTest.Domain.Model
{
    public class Sequence
    {
        [Key]
        public int sequence_id { get; set; }
        public int sequence_line { get; set; }
        public string sra_id { get; set; }
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
