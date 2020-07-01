using SerratusTest.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserNs
{
    public class ParsedFile
    {
        public Run Run { get; set; } = new Run();
        public List<FamilySection> FamilySections { get; set; } = new List<FamilySection>();
        public List<AccessionSection> AccessionSections { get; set; } = new List<AccessionSection>();
        public List<FastaSection> FastaSections { get; set; } = new List<FastaSection>();
    }
}
