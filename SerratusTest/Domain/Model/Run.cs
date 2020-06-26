using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerratusTest.Domain.Model
{
    public class Run
    {
        public int RunId { get; set; }
        public string Sra { get; set; }
        public string Genome { get; set; }
        public string Date { get; set; }
        public IList<FamilySection> FamilySections { get; set; }
        public IList<AccessionSection> AccessionSections { get; set; }
        public IList<FastaSection> FastaSections { get; set; }
    }
}
