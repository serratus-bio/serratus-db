using System;
using System.Collections.Generic;
using System.Text;

namespace ParserNs
{
    public class UnparsedFile
    {
        public string Sra { get; set; } = "";
        public string CommentLineFromFile { get; set; } = "";
        public List<string> FamilyLinesFromFile { get; set; } = new List<string>();
        public List<string> SequenceLinesFromFile { get; set; } = new List<string>();
    }
}
