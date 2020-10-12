using SerratusTest.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserNs
{
    public class ParsedFile
    {
        public Run run { get; set; } = new Run();
        public List<Family> family { get; set; } = new List<Family>();
        public List<Sequence> sequence { get; set; } = new List<Sequence>();
    }
}
