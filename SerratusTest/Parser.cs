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
using Amazon;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ParserNs
{
    public class Parser
    {
        private readonly CancellationToken cancellationToken;
        private readonly string _accessKey;
        private readonly string _secretKey;
        
        public string ContinuationToken { get; set; } = "";
        public List<string> SummaryFiles { get; set; }

        public Parser(string accessKey, string secretKey)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
        }

        public void GetBucketsFromS3()
        {

            Console.WriteLine("Parser Started");
            var moreKeys = true;
            SummaryFiles = new List<string>();
            if ( ContinuationToken == "")
            {
                S3Object obj = new S3Object();
                AmazonS3Config config = new AmazonS3Config();
                config.RegionEndpoint = RegionEndpoint.USEast1;
                AmazonS3Client s3Client = new AmazonS3Client(
                        _accessKey,
                        _secretKey,
                        config
                        );
                var request = new ListObjectsV2Request { BucketName = "lovelywater", Prefix = "summary" };
                Task<ListObjectsV2Response> buckets = s3Client.ListObjectsV2Async(request);
                var res = buckets.Result;
                ContinuationToken = res.NextContinuationToken;
                res.S3Objects.ForEach(obj => SummaryFiles.Add(obj.Key));
                Console.WriteLine($"{SummaryFiles.Count}");
                GetDataFromBucketList(moreKeys).Wait();
            }
            else
            {
                SummaryFiles = new List<string>();
                S3Object obj = new S3Object();
                AmazonS3Config config = new AmazonS3Config();
                AmazonS3Client s3Client = new AmazonS3Client(
                        _accessKey,
                        _secretKey,
                        config
                        );
                var request = new ListObjectsV2Request { BucketName = "lovelywater", Prefix = "summary", ContinuationToken = ContinuationToken };
                Task<ListObjectsV2Response> buckets = s3Client.ListObjectsV2Async(request);
                var res = buckets.Result;
                ContinuationToken = res.NextContinuationToken;
                res.S3Objects.ForEach(obj => SummaryFiles.Add(obj.Key));
                if (res.KeyCount < 1000)
                {
                    moreKeys = false;
                    GetDataFromBucketList(moreKeys).Wait();
                    return;
                }
                else
                {
                    GetDataFromBucketList(moreKeys).Wait();
                }
            }
        }

        public async Task GetDataFromBucketList(bool moreKeys)
        {
            Console.WriteLine($"get data from bucket started");
            AmazonS3Config config = new AmazonS3Config();
            AmazonS3Client s3Client = new AmazonS3Client(
                    _accessKey,
                    _secretKey,
                    config
                    );
            var oneFile = Stopwatch.StartNew();
            foreach ( string fileName in SummaryFiles )
            {
                var request = new GetObjectRequest
                {
                    BucketName = "lovelywater",
                    Key = $"{fileName}"
                };

                string responseBody = "";
                var s3Stopwatch = Stopwatch.StartNew();
                var result = await s3Client.GetObjectAsync(request);
                //Console.WriteLine($"{s3Stopwatch.ElapsedMilliseconds}");
                using (var stream = result.ResponseStream)
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine($"file read");
                    var readerStopwatch = Stopwatch.StartNew();
                    responseBody = await reader.ReadToEndAsync();
                    //Console.WriteLine($"{readerStopwatch.ElapsedMilliseconds}");
                    _ = Task.Run(async () =>
                    { 
                        using (var context = new SerratusSummaryContext())
                        {
                            
                            var parserStopwatch = Stopwatch.StartNew();
                            var lines = responseBody.Split('\n');
                            var unparsedFile = ReadFile(lines);
                            var finishedFile = ParseFile(fileName, unparsedFile);
                            Console.WriteLine($"file parsed");
                            //Console.WriteLine($"{parserStopwatch.ElapsedMilliseconds}");
                            var dbStopwatch = Stopwatch.StartNew();
                            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                            context.Runs.Add(finishedFile.Run);
                            await context.SaveChangesAsync();
                            Console.WriteLine($"file saved in db");
                            //Console.WriteLine($"{dbStopwatch.ElapsedMilliseconds}");
                        }
                    });
                }
            }
            if (moreKeys == true)
            {
                Console.WriteLine("getting more buckets");
                GetBucketsFromS3();
            }
            else
            {
                return;
            }
        }

        public UnparsedFile ReadFile(string [] lines)
        {
            var unparsedFile = new UnparsedFile();
            foreach (string line in lines)
            {
                if (line.StartsWith("S")) unparsedFile.CommentLineFromFile = line;
                else if (line.StartsWith("f")) unparsedFile.FamilyLinesFromFile.Add(line);
                else if (line.StartsWith("a")) unparsedFile.AccessionLinesFromFile.Add(line);
                else if (line.StartsWith(">") || line.StartsWith("A") || line.StartsWith("T") || line.StartsWith("C") || line.StartsWith("G")) unparsedFile.FastaLinesFromFile.Add(line);
            }
            return unparsedFile;
        }

        public ParsedFile ParseFile(string fileName, UnparsedFile unparsedFile)
        {
            var partiallyParsedFileC = ParseCommentLine(fileName, unparsedFile);
            var partiallyParsedFileCF = ParseFamilySection(partiallyParsedFileC, unparsedFile);
            var partiallyParsedFileCFA = ParseAccessionSection(partiallyParsedFileCF, unparsedFile);
            var parsedFile = ParseFastaSection(partiallyParsedFileCFA, unparsedFile);
            var finishedFile = CreateDbEntry(parsedFile);
            return finishedFile;
        }

        public ParsedFile CreateDbEntry(ParsedFile parsedFile)
        {
            var finishedFile = parsedFile;
            finishedFile.Run.AccessionSections = parsedFile.AccessionSections;
            finishedFile.Run.FamilySections = parsedFile.FamilySections;
            finishedFile.Run.FastaSections = parsedFile.FastaSections;
            return finishedFile;
        }

        public ParsedFile ParseCommentLine(string fileName, UnparsedFile unparsedFile)
        {
            string[] split = unparsedFile.CommentLineFromFile.Split(new char[] { ',' });
            string[] sra = split[0].Split(new char[] { '=' });
            string[] gen = split[1].Split(new char[] { '=' });
            string[] date = split[2].Split(new char[] { '=' });
            unparsedFile.Sra = sra[2];
            var run = new Run();
            run.FileName = fileName;
            run.Sra = sra[2];
            run.Genome = gen[1];
            run.Date = date[1];
            var partiallyParsedFile = new ParsedFile();
            partiallyParsedFile.Run = run;
            return partiallyParsedFile;
        }
        
        public FamilySection ParseFamilySectionLine(string[] line, int lineId, string sra)
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
            var familySection = new FamilySection
            {
                FamilySectionLineId = familySectionLineId,
                Sra = sra,
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
            };
            return familySection;
        }

        public AccessionSection ParseAccessionSectionLine(string[] line, int lineId, string sra)
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
            var accessionSection = new AccessionSection
            {
                AccessionSectionLineId = accessionSectionLineId,
                Acc = acc,
                Sra = sra,
                Fam = fam,
                PctId = pctId,
                Aln = aln,
                Glb = glb,
                Len = len,
                CvgPct = cvgPct,
                Depth = depth,
                Cvg = cvg,
                Name = name,
            };
            return accessionSection;
        }

        public ParsedFile ParseFamilySection(ParsedFile partiallyParsedFileC, UnparsedFile unparsedFile)
        {
            int i = 1;
            string[] temp;
            foreach (string line in unparsedFile.FamilyLinesFromFile)
            {
                temp = line.Split(new char[] { ';' });
                var familySection = ParseFamilySectionLine(temp, i, unparsedFile.Sra);
                partiallyParsedFileC.FamilySections.Add(familySection);
                i++;
            }
            var partiallyParsedFileCF = partiallyParsedFileC;
            return partiallyParsedFileCF;
        }

        public ParsedFile ParseAccessionSection(ParsedFile partiallyParsedFileCF, UnparsedFile unparsedFile)
        {
            int i = 1;
            string[] temp;
            foreach (string line in unparsedFile.AccessionLinesFromFile)
            {
                temp = line.Split(new char[] { ';' });
                var accessionSection = ParseAccessionSectionLine(temp, i, unparsedFile.Sra);
                partiallyParsedFileCF.AccessionSections.Add(accessionSection);
                i++;
            }
            var partiallyParsedFileCFA = partiallyParsedFileCF;
            return partiallyParsedFileCFA;
        }

        public ParsedFile ParseFastaSection(ParsedFile partiallyParsedFileCFA, UnparsedFile unparsedFile)
        {
            bool first = true;
            bool second = false;
            int i = 1;
            int j = 0;
            List<string> firstLine = new List<string>();
            List<string> secondLine = new List<string>();
            List<int> lineNumber = new List<int>();
            foreach (string line in unparsedFile.FastaLinesFromFile)
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
                    partiallyParsedFileCFA.FastaSections.Add(new FastaSection
                    {
                        FastaSectionLineId = lineNumber[j],
                        Sra = unparsedFile.Sra,
                        SequenceId = firstLine[j],
                        Sequence = secondLine[j]
                    });
                    j++;
                }
                second = !second;
            }
            var parsedFile = partiallyParsedFileCFA;
            return parsedFile;
        }
    }
}
