using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerratusTest.Domain.Model
{
    public class FamilySection
    {
        public int FamilySectionId { get; set; }
        public int FamilySectionLineId { get; set; }
        public string Family { get; set; }
        public int Score { get; set; }
        public int PctId { get; set; }
        public int Aln { get; set; }
        public int Glb { get; set; }
        public int PanLen { get; set; }
        public string Cvg { get; set; }
        public string Top { get; set; }
        public int TopAln { get; set; }
        public int TopLen { get; set; }
        public string TopName { get; set; }
        public int CommentLineId { get; set; }
    }
}
