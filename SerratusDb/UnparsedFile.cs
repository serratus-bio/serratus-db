using System;
using System.Collections.Generic;
using System.Text;

namespace ParserNs
{
    public class UnparsedFile
    {
        public string CommentLineFromFile { get; set; } = "";
        public string Sra { get; set; } = "";
        public List<string> FamilyLinesFromFile { get; set; } = new List<string>();
        public List<string> AccessionLinesFromFile { get; set; } = new List<string>();
        public List<string> FastaLinesFromFile { get; set; } = new List<string>();
    }
}
