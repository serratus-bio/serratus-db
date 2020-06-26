using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore.Internal;
using System.Runtime.InteropServices;
using SerratusTest.Domain.Model;
using SerratusTest.ORM;
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading;

namespace ParserNs
{
    public class Parser
    {
        public string CommentLineFromFile { get; set; }
        public string Sra { get; set; }

        private readonly CancellationToken cancellationToken;
        public List<string> SummaryFiles { get; set; } = new List<string>();

        public Run Run { get; set; } = new Run();
        public List<FamilySection> FamilySections { get; set; } = new List<FamilySection>();
        public List<AccessionSection> AccessionSections { get; set; } = new List<AccessionSection>();
        public List<FastaSection> FastaSections { get; set; } = new List<FastaSection>();

        public List<string> FamilyLinesFromFile { get; set; }
        public List<string> AccessionLinesFromFile { get; set; }
        public List<string> FastaLinesFromFile { get; set; }

        static readonly string textfile = @"C:\Users\Dan\Desktop\tettest.txt";

        public Parser()
        {
            FamilyLinesFromFile = new List<string>();
            AccessionLinesFromFile = new List<string>();
            FastaLinesFromFile = new List<string>();
        }

        public void GetBucketsFromS3()
        {
            S3Object obj = new S3Object();

            AmazonS3Config config = new AmazonS3Config();

            AmazonS3Client s3Client = new AmazonS3Client(
                    accessKey,
                    secretKey,
                    config
                    );
            var request = new ListObjectsV2Request{ BucketName = "lovelywater", Prefix = "summary" };
            Task<ListObjectsV2Response> buckets = s3Client.ListObjectsV2Async(request);
            var res = buckets.Result;
            res.S3Objects.ForEach(obj => SummaryFiles.Add(obj.Key));
            Console.WriteLine("Success!");
            GetDataFromBucketList();
        }

        public void GetDataFromBucketList()
        {
            AmazonS3Config config = new AmazonS3Config();

            AmazonS3Client s3Client = new AmazonS3Client(
                    accessKey,
                    secretKey,
                    config
                    );
            foreach ( string file in SummaryFiles )
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = "lovelywater",
                    Key = $"{file}"
                };

                string responseBody = "";

                using (Task<GetObjectResponse> res = s3Client.GetObjectAsync(request))
                using (Stream stream = res.Result.ResponseStream)
                using(StreamReader reader = new StreamReader(stream))
                { 
                    responseBody = reader.ReadToEnd();
                    var lines = responseBody.Split('\n');
                    ReadFile(lines);
                    ParseFile();
                }
                
                using (var context = new SerratusSummaryContext())
                {
                    context.Runs.Add(Run);
                    context.SaveChanges();
                }
                Console.WriteLine("Success!");
            }
        }

        public string[] ReadFile(string [] lines)
        {
            //var lines = File.ReadAllLines(textfile);
            foreach (string line in lines)
            {
                if (line.StartsWith("S")) CommentLineFromFile = line;
                else if (line.StartsWith("f")) FamilyLinesFromFile.Add(line);
                else if (line.StartsWith("a")) AccessionLinesFromFile.Add(line);
                else if (line.StartsWith(">") || line.StartsWith("A") || line.StartsWith("T") || line.StartsWith("C") || line.StartsWith("G")) FastaLinesFromFile.Add(line);
            }
            return lines;
        }

        public void ParseFile()
        {
            Run = new Run();
            AccessionSections = new List<AccessionSection>();
            FamilySections = new List<FamilySection>();
            FastaSections = new List<FastaSection>();
            ParseCommentLine();
            ParseFamilySection();
            ParseAccessionSection();
            ParseFastaSection();
            CreateDbEntry();
        }

        public void CreateDbEntry()
        {
            Run.AccessionSections = AccessionSections;
            Run.FamilySections = FamilySections;
            Run.FastaSections = FastaSections;
        }

        public Run ParseCommentLine()
        {
            string[] split = CommentLineFromFile.Split(new char[] { ',' });
            string[] sra = split[0].Split(new char[] { '=' });
            string[] gen = split[1].Split(new char[] { '=' });
            string[] date = split[2].Split(new char[] { '=' });
            Sra = sra[2];
            Run.Sra = sra[2];
            Run.Genome = gen[1];
            Run.Date = date[1];
            return Run;
        }
        
        public void ParseFamilySectionLine(string[] line, int lineId)
        {
            int familySectionLineId = lineId;
            string family = line[0].Split(new char[] { '=' })[1];
            int score = int.Parse(line[1].Split(new char[] { '=' })[1]);
            int pctId = int.Parse(line[2].Split(new char[] { '=' })[1]);
            int aln = int.Parse(line[3].Split(new char[] { '=' })[1]);
            int glb = int.Parse(line[4].Split(new char[] { '=' })[1]);
            int panLen = int.Parse(line[5].Split(new char[] { '=' })[1]);
            string cvg = line[6].Split(new char[] { '=' })[1];
            string top = line[7].Split(new char[] { '=' })[1];
            int topAln = int.Parse(line[8].Split(new char[] { '=' })[1]);
            int topLen = int.Parse(line[9].Split(new char[] { '=' })[1]);
            string topName = line[10].Split(new char[] { '=' })[1];
            FamilySections.Add(new FamilySection{
                FamilySectionLineId = familySectionLineId,
                Sra = Sra,
                Family = family,
                Score = score,
                PctId = pctId,
                Aln = aln,
                Glb = glb,
                PanLen = panLen,
                Cvg = cvg,
                Top = top,
                TopAln = topAln,
                TopLen = topLen,
                TopName = topName,
            });
        }

        public void ParseAccessionSectionLine(string[] line, int lineId)
        {
            int accessionSectionLineId = lineId;
            string acc = line[0].Split(new char[] { '=' })[1];
            double pctId = double.Parse(line[1].Split(new char[] { '=' })[1]);
            int aln = int.Parse(line[2].Split(new char[] { '=' })[1]);
            int glb = int.Parse(line[3].Split(new char[] { '=' })[1]);
            int len = int.Parse(line[4].Split(new char[] { '=' })[1]);
            int cvgPct = int.Parse(line[5].Split(new char[] { '=' })[1]);
            double depth = double.Parse(line[7].Split(new char[] { '=' })[1]);
            string cvg = line[8].Split(new char[] { '=' })[1];
            string fam = line[9].Split(new char[] { '=' })[1];
            string name = line[10].Split(new char[] { '=' })[1];
            AccessionSections.Add(new AccessionSection
            {
                AccessionSectionLineId = accessionSectionLineId,
                Acc = acc,
                Sra = Sra,
                Fam = fam,
                PctId = pctId,
                Aln = aln,
                Glb = glb,
                Len = len,
                CvgPct = cvgPct,
                Depth = depth,
                Cvg = cvg,
                Name = name,
            });
        }

        public void ParseFamilySection()
        {
            int i = 1;
            string[] temp;
            foreach (string line in FamilyLinesFromFile)
            {
                temp = line.Split(new char[] { ';' });
                ParseFamilySectionLine(temp, i);
                i++;
            }
        }

        public void ParseAccessionSection()
        {
            int i = 1;
            string[] temp;
            foreach (string line in AccessionLinesFromFile)
            {
                temp = line.Split(new char[] { ';' });
                ParseAccessionSectionLine(temp, i);
                i++;
            }
        }

        public void ParseFastaSection()
        {
            bool first = true;
            bool second = false;
            int i = 1;
            int j = 0;
            List<string> firstLine = new List<string>();
            List<string> secondLine = new List<string>();
            List<int> lineNumber = new List<int>();
            foreach (string line in FastaLinesFromFile)
            {
                if (first)
                {
                    firstLine.Add(line);
                    first = !first;
                }
                if (second)
                {
                    secondLine.Add(line);
                    lineNumber.Add(i);
                    i++;
                    first = !first;
                    FastaSections.Add(new FastaSection
                    {
                        FastaSectionLineId = lineNumber[j],
                        Sra = Sra,
                        SequenceId = firstLine[j],
                        Sequence = secondLine[j]
                    });
                    j++;
                }
                second = !second;
            }
        }
    }
}
