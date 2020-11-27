using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SerratusTest.Domain.Model
{
    public class Run
    {
        [Key]
        public int run_id { get; set; }
        public string file_name { get; set; }
        public string sra_id { get; set; }
        public string date { get; set; }
        public string version { get; set; }
        public string genome { get; set; }
        public string read_length { get; set; }
        [ForeignKey("run_id")]
        public IList<Family> family { get; set; }
        [ForeignKey("run_id")]
        public IList<Sequence> sequence { get; set; }
    }
}
