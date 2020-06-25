using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerratusTest.Domain.Model
{
    public class FastaSection
    {
        public int FastaSectionId { get; set; }
        public int FastaSectionLineId { get; set; }
        public string SequenceId { get; set; }
        public string Sequence { get; set; }
        public int CommentLineId { get; set; }
    }
}
