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
        private readonly string _accessKey;
        private readonly string _secretKey;
        
        public string ContinuationToken { get; set; } = "";
        public List<string> SummaryFiles { get; set; }

        public Parser(string accessKey, string secretKey)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
        }

        static readonly string textfile = @"/home/ec2-user/Summary10.txt";

        public List<string> FileOne { get; set; } = new List<string>();
        public List<string> FileTwo { get; set; } = new List<string>();
        public List<string> FileThree { get; set; } = new List<string>();
        public List<string> FileFour { get; set; } = new List<string>();
        public List<string> FileFive { get; set; } = new List<string>();
        public List<string> FileSix { get; set; } = new List<string>();
        public List<string> FileSeven { get; set; } = new List<string>();
        public List<string> FileEight { get; set; } = new List<string>();
        public List<string> FileNine { get; set; } = new List<string>();
        public List<string> FileTen { get; set; } = new List<string>();

        public List<string> ReadFile()
        {
            SummaryFiles = new List<string>();
            var lines = File.ReadAllLines(textfile);
            foreach (string line in lines)
            {
                SummaryFiles.Add(line);
            }
            Console.WriteLine("File read");
            return SummaryFiles;
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
                //GetDataFromBucketList(moreKeys).Wait();
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
                    //GetDataFromBucketList(moreKeys).Wait();
                    return;
                }
                else
                {
                    //GetDataFromBucketList(moreKeys).Wait();
                }
            }
        }

        public async Task GetDataFromBucketList()
        {
            ReadFile();
            Console.WriteLine($"get data from bucket started");
            AmazonS3Config config = new AmazonS3Config();
            AmazonS3Client s3Client = new AmazonS3Client(
                    _accessKey,
                    _secretKey,
                    config
                    );
            var oneFile = Stopwatch.StartNew();
            var tasks = new List<Task>();
            foreach ( string fileName in SummaryFiles )
            {
                var request = new GetObjectRequest
                {
                    BucketName = "lovelywater",
                    Key = $"{fileName}"
                };

                string responseBody = "";
                var s3Stopwatch = Stopwatch.StartNew();
                try
                {
                    Console.WriteLine("test");
                    var result = await s3Client.GetObjectAsync(request);
                    using (var stream = result.ResponseStream)
                    using (var reader = new StreamReader(stream))
                    {
                        Console.WriteLine($"file read");
                        var readerStopwatch = Stopwatch.StartNew();
                        responseBody = await reader.ReadToEndAsync();
                        //Console.WriteLine($"{readerStopwatch.ElapsedMilliseconds}");
                        var task = Task.Run(async () =>
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
                                context.run.Add(finishedFile.run);
                                await context.SaveChangesAsync();
                                Console.WriteLine($"file saved in db");
                                //Console.WriteLine($"{dbStopwatch.ElapsedMilliseconds}");
                            }
                        });
                        tasks.Add(task);
                    }
                }
                catch
                {
                    continue;
                }
                if (tasks.Count == 100)
                {
                    await Task.WhenAll(tasks);
                    Console.WriteLine("tasks done!");
                }
                //Console.WriteLine($"{s3Stopwatch.ElapsedMilliseconds}");
            }
            Console.WriteLine("PARSER COMPLETE");
            //if (moreKeys == true)
            //{
            //    Console.WriteLine("getting more buckets");
            //    GetBucketsFromS3();
            //}
            //else
            //{
            //    return;
            //}
        }

        public UnparsedFile ReadFile(string [] lines)
        {
            var unparsedFile = new UnparsedFile();
            foreach (string line in lines)
            {
                if (line.StartsWith("r"))
                {
                    unparsedFile.CommentLineFromFile = line;
                }
                else if (line.StartsWith("f"))
                {
                    unparsedFile.FamilyLinesFromFile.Add(line);
                } 
                else if (line.StartsWith("s"))
                {
                    unparsedFile.SequenceLinesFromFile.Add(line);
                }
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
            finishedFile.run.family = parsedFile.family;
            finishedFile.run.sequence = parsedFile.sequence;
            return finishedFile;
        }

        public ParsedFile ParseCommentLine(string fileName, UnparsedFile unparsedFile)
        {
            string[] split = unparsedFile.CommentLineFromFile.Split(new char[] { ',' });
            string[] sra = split[0].Split(new char[] { '=', ';' });
            string[] genome = split[1].Split(new char[] { '=' });
            string[] version = split[2].Split(new char[] { '=' });
            string[] date = split[3].Split(new char[] { '=' });
            unparsedFile.Sra = sra[4];
            var run = new Run
            {
                file_name = fileName,
                sra_id = sra[4],
                genome = genome[1],
                version = version[1],
                date = date[1]
            };
            var partiallyParsedFile = new ParsedFile
            {
                run = run
            };
            return partiallyParsedFile;
        }
        
        public Family ParseFamilySectionLine(string[] line, int line_number, string sra)
        {
            int family_line = line_number;
            string family_name = line[1].Split(new char[] { '=' })[1];
            int score = int.Parse(line[2].Split(new char[] { '=' })[1]);
            int percent_identity = int.Parse(line[3].Split(new char[] { '=' })[1]);
            string coverage_bins = line[0].Split(new char[] { '=' })[1];
            int n_reads = int.Parse(line[5].Split(new char[] { '=' })[1]);
            int n_global_reads = int.Parse(line[6].Split(new char[] { '=' })[1]);
            int length = int.Parse(line[7].Split(new char[] { '=' })[1]);
            double depth = double.Parse(line[4].Split(new char[] { '=' })[1]);
            string top_genbank_id = line[8].Split(new char[] { '=' })[1];
            int top_length = int.Parse(line[10].Split(new char[] { '=' })[1]);
            int top_score = int.Parse(line[9].Split(new char[] { '=' })[1]);
            string top_name = line[11].Split(new char[] { '=' })[1];
            var familySection = new Family
            {
                family_line = family_line,
                sra_id = sra,
                family_name = family_name,
                score = score,
                percent_identity = percent_identity,
                coverage_bins = coverage_bins,
                n_reads = n_reads,
                n_global_reads = n_global_reads,
                length = length,
                depth = depth,
                top_genbank_id = top_genbank_id,
                top_name = top_name,
                top_score = top_score,
                top_length = top_length,
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
                var family = ParseFamilySectionLine(temp, i, unparsedFile.Sra);
                partiallyParsedFileC.family.Add(family);
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
