using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SerratusTest.Domain.Model;
using SerratusTest.ORM;

namespace SerratusTest.Services
{
    public class SerratusSummaryService : ISerratusSummaryService
    {
        private readonly SerratusSummaryContext _serratusSummaryContext;
        public SerratusSummaryService(SerratusSummaryContext serratusSummaryContext)
        {
            _serratusSummaryContext = serratusSummaryContext;
        }

        public void AddEntry()
        {
            _serratusSummaryContext.CommentLines.Add(new CommentLine
            {
                CommentLineId = 1,
                Sra = "ERR2756788",
                Genome = "cov3ma",
                Date = "200607 - 01:47"
            });
            _serratusSummaryContext.SaveChanges();
        }
    }
}
