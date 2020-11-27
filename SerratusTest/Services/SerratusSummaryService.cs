using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SerratusTest.Domain.Model;
using SerratusTest.ORM;

namespace SerratusTest.Services
{
    public class SerratusSummaryService  /* : ISerratusSummaryService*/
    {
        private readonly SerratusSummaryContext _serratusSummaryContext;
        public SerratusSummaryService(SerratusSummaryContext serratusSummaryContext)
        {
            _serratusSummaryContext = serratusSummaryContext;
        }

        //public void AddRun()
        //{
        //    _serratusSummaryContext.Runs.Add(new Run
        //    {
        //        RunId = 1,
        //        Sra = "ERR2756788",
        //        Genome = "cov3ma",
        //        Date = "200607 - 01:47"
        //    });
        //    _serratusSummaryContext.SaveChanges();
        //}
        //public void AddFamilySection()
        //{
        //    _serratusSummaryContext.FamilySections.Add(new FamilySection
        //    {
        //        FamilySectionId = 1,
        //        FamilySectionLineId = 1,
        //        Family = "AMR",
        //        Score = 100,
        //        PctId = 97,
        //        Aln = 1382,
        //        Glb = 845,
        //        PanLen = 1000,
        //        Cvg = "Ooo.oooOOOOOOoooo..o....O",
        //        Top = "AY874537_3000883",
        //        TopAln = 570,
        //        TopLen = 861,
        //        TopName = "TEM - 11 Proteus mirabilis",
        //        RunId = 1,
        //    });
        //    _serratusSummaryContext.SaveChanges();
        //}
        //public void AddAccessionSection()
        //{
        //    _serratusSummaryContext.AccessionSections.Add(new AccessionSection
        //    {
        //        AccessionSectionId = 1,
        //        AccessionSectionLineId = 1,
        //        Acc = "AY874537_3000883",
        //        PctId = 98.1,
        //        Aln = 570,
        //        Glb = 432,
        //        Len = 861,
        //        CvgPct = 96,
        //        Depth = 92.2,
        //        Cvg = "Oo.ooOOOoOOOOOOOOOOOO.Oo_",
        //        Fam = "AMR",
        //        Name = "TEM-11,Proteus mirabilis",
        //        RunId = 1,
        //    });
        //    _serratusSummaryContext.SaveChanges();
        //}
    }
}
