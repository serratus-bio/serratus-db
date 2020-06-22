using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerratusTest.Domain.Model
{
    public class AccessionSection
    {
        public int AccessionSectionId { get; set; }
        public int AccessionSectionLineId { get; set; }
        public string Acc { get; set; }
        public int PctId { get; set; }
        public int Aln { get; set; }
        public int Glb { get; set; }
        public int Len { get; set; }
        public string CvgPct { get; set; }
        public int Depth { get; set; }
        public string Cvg { get; set; }
        public string Fam { get; set; }
        public string Name { get; set; }
        public int CommentLineId { get; set; }
        public int FamilySectionId { get; set; }
    }
}
