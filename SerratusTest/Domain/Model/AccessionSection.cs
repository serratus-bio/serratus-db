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
        public int RunId { get; set; }
        public string Sra { get; set; }
        public string Fam { get; set; }
        public string Acc { get; set; }
        public double PctId { get; set; }
        public int Aln { get; set; }
        public int Glb { get; set; }
        public int Len { get; set; }
        public int CvgPct { get; set; }
        public double Depth { get; set; }
        public string Cvg { get; set; }
        public string Name { get; set; }
    }
}
