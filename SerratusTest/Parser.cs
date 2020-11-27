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
using Microsoft.VisualBasic.CompilerServices;

namespace ParserNs
{
    public class Parser
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        
        public List<string> SummaryFiles { get; set; } = new List<string>();
        public Parser(string accessKey, string secretKey)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
        }
        //static readonly string textfile = @"/home/ec2-user/Summary10.txt";
        static readonly string textfile = @"Summary.txt";

        public List<string> ReadAwsFile()
        {
            var lines = File.ReadAllLines(textfile);
            foreach (string line in lines)
            {
                SummaryFiles.Add(line);
            }
            Console.WriteLine("File read");
            return SummaryFiles;
        }

        public async Task GetDataFromFileList()
        {
            ReadAwsFile();
            Console.WriteLine($"Files loaded for parsing.");
            AmazonS3Config config = new AmazonS3Config();
            config.RegionEndpoint = RegionEndpoint.USEast1;
            AmazonS3Client s3Client = new AmazonS3Client(
                    _accessKey,
                    _secretKey,
                    config
                    );
            Console.WriteLine($"S3 Client created.");
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
                    GetObjectResponse r = await s3Client.GetObjectAsync(request);
                    using (var stream = r.ResponseStream)
                    using (var reader = new StreamReader(stream))
                    {
                        Console.WriteLine($"File read.");
                        var readerStopwatch = Stopwatch.StartNew();
                        responseBody = await reader.ReadToEndAsync();
                        //Console.WriteLine($"{readerStopwatch.ElapsedMilliseconds}");
                        var task = Task.Run(async () =>
                        {
                            using (var context = new SerratusSummaryContext())
                            {
                                var parserStopwatch = Stopwatch.StartNew();
                                var lines = responseBody.Split('\n');
                                var unparsedFile = AddUnparsedFile(lines);
                                var finishedFile = ParseFile(fileName, unparsedFile);
                                Console.WriteLine($"File parsed.");
                                //Console.WriteLine($"{parserStopwatch.ElapsedMilliseconds}");
                                var dbStopwatch = Stopwatch.StartNew();
                                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                                context.run.Add(finishedFile.run);
                                await context.SaveChangesAsync();
                                Console.WriteLine($"File saved in db.");
                                //Console.WriteLine($"{dbStopwatch.ElapsedMilliseconds}");
                            }
                        });
                        tasks.Add(task);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                if (tasks.Count == 100)
                {
                    await Task.WhenAll(tasks);
                    Console.WriteLine("100 Tasks done!");
                }
                //Console.WriteLine($"{s3Stopwatch.ElapsedMilliseconds}");
            }
        }

        public UnparsedFile AddUnparsedFile(string[] lines)
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
            var parsedFile = ParseSequenceSection(partiallyParsedFileCF, unparsedFile);
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
                date = date[1].Split(new char[] { ';' })[0],
                read_length = sra[1]
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

        public Sequence ParseSequenceSectionLine(string[] line, int line_number, string sra)
        {
            int sequence_line = line_number;
            string genbank_id = line[1].Split(new char[] { '=' })[1];
            string genbank_name = line[9].Split(new char[] { '=' })[1];
            string family_name = line[8].Split(new char[] { '=' })[1];
            int score = int.Parse(line[2].Split(new char[] { '=' })[1]);
            int percent_identity = int.Parse(line[3].Split(new char[] { '=' })[1]);
            string coverage_bins = line[0].Split(new char[] { '=' })[1];
            int n_reads = int.Parse(line[5].Split(new char[] { '=' })[1]);
            int n_global_reads = int.Parse(line[6].Split(new char[] { '=' })[1]);
            int length = int.Parse(line[7].Split(new char[] { '=' })[1]);
            double depth = double.Parse(line[4].Split(new char[] { '=' })[1]);
            var sequence = new Sequence
            {
                sequence_line = sequence_line,
                genbank_id = genbank_id,
                genbank_name = genbank_name,
                family_name = family_name,
                score = score,
                percentage_identity = percent_identity,
                n_reads = n_reads,
                n_global_reads = n_global_reads,
                length = length,
                coverage_bins = coverage_bins,
                depth = depth,
                sra_id = sra
            };
            return sequence;
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

        public ParsedFile ParseSequenceSection(ParsedFile partiallyParsedFileCF, UnparsedFile unparsedFile)
        {
            int i = 1;
            string[] temp;
            foreach (string line in unparsedFile.SequenceLinesFromFile)
            {
                temp = line.Split(new char[] { ';' });
                var sequenceSection = ParseSequenceSectionLine(temp, i, unparsedFile.Sra);
                partiallyParsedFileCF.sequence.Add(sequenceSection);
                i++;
            }
            var partiallyParsedFileCFA = partiallyParsedFileCF;
            return partiallyParsedFileCFA;
        }
    }
}
