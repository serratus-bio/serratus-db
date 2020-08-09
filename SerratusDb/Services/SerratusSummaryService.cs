using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SerratusDb.Domain.Model;
using SerratusDb.ORM;

namespace SerratusDb.Services
{
    public class SerratusSummaryService : ISerratusSummaryService
    {
        private readonly SerratusSummaryContext _context;

        public SerratusSummaryService(SerratusSummaryContext serratusSummaryContext)
        {
            _context = serratusSummaryContext;
        }

        // Runs Section

        public void AddRun()
        {
            _context.Runs.Add(new Run
            {
                RunId = 1,
                Sra = "ERR2756788",
                Genome = "cov3ma",
                Date = "200607 - 01:47"
            });
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Run>> GetRuns()
        {
            return await _context.Runs.ToListAsync();
        }

        public async Task<Run> GetSummaryForSraAccession(string run)
        {

            var sraAccession = await _context.Runs.FirstOrDefaultAsync(r => r.Sra == run);

            var family = await _context.FamilySections
                .Where(f => f.RunId == sraAccession.RunId)
                .OrderBy(f => f.FamilySectionLineId)
                .ToListAsync();

            var accs = await _context.AccessionSections
                .Where(a => a.RunId == sraAccession.RunId)
                .OrderBy(a => a.AccessionSectionLineId)
                .ToListAsync();

            var fasta = await _context.FastaSections
                .Where(f => f.RunId == sraAccession.RunId)
                .OrderBy(f => f.FastaSectionLineId)
                .ToListAsync();

            sraAccession.FamilySections = family;
            sraAccession.AccessionSections = accs;
            sraAccession.FastaSections = fasta;

            return sraAccession;
        }

        public async Task<Run> GetRun(int id)
        {
            var run = await _context.Runs.FindAsync(id);

            return run;
        }

        public async void PutRun(int id, Run run)
        {
            if (id != run.RunId)
            {
                return;
            }

            _context.Entry(run).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RunExists(id))
                {
                    return;
                }
                else
                {
                    throw;
                }
            }

            return;
        }

        public async Task<Run> PostRun(Run run)
        {
            _context.Runs.Add(run);
            await _context.SaveChangesAsync();

            return run;
        }

        public async void DeleteRun(int id)
        {
            var run = await _context.Runs.FindAsync(id);
            if (run == null)
            {
                return;
            }

            _context.Runs.Remove(run);
            await _context.SaveChangesAsync();

            return;
        }

        private bool RunExists(int id)
        {
            return _context.Runs.Any(e => e.RunId == id);
        }


        // Family Section
        public void AddFamilySection()
        {
            _context.FamilySections.Add(new FamilySection
            {
                FamilySectionId = 1,
                FamilySectionLineId = 1,
                Family = "AMR",
                Score = 100,
                PctId = 97,
                Aln = 1382,
                Glb = 845,
                PanLen = 1000,
                Cvg = "Ooo.oooOOOOOOoooo..o....O",
                Top = "AY874537_3000883",
                TopAln = 570,
                TopLen = 861,
                TopName = "TEM - 11 Proteus mirabilis",
                RunId = 1,
            });
            _context.SaveChanges();
        }

        public async Task<IEnumerable<FamilySection>> GetRunsFromFamily(string family)
        {
            using var context = _context;

            var families = context.FamilySections
            .Where(f => f.Family == family)
            .OrderByDescending(f => f.Score)
            .Take(100)
            .ToListAsync();

            return await families;
        }

        public async Task<IEnumerable<FamilySection>> GetFamilySections()
        {
            
            return  await _context.FamilySections.ToListAsync();
        }

        public async Task<FamilySection> GetFamilySection(int id)
        {
            return await _context.FamilySections.FindAsync(id);

        }

        public async void PutFamilySection(int id, FamilySection familySection)
        {
            if (id != familySection.FamilySectionId)
            {
                return ;
            }

            _context.Entry(familySection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilySectionExists(id))
                {
                    return ;
                }
                else
                {
                    throw;
                }
            }

            return;
        }

        public async Task<FamilySection> PostFamilySection(FamilySection familySection)
        {
            _context.FamilySections.Add(familySection);
            await _context.SaveChangesAsync();

            return familySection;
        }


        public async void DeleteFamilySection(int id)
        {
            var familySection = await _context.FamilySections.FindAsync(id);
            if (familySection == null)
            {
                return;
            }

            _context.FamilySections.Remove(familySection);
            await _context.SaveChangesAsync();

            return;
        }

        private bool FamilySectionExists(int id)
        {
            return _context.FamilySections.Any(e => e.FamilySectionId == id);
        }

        // Accession Secction

        public void AddAccessionSection()
        {
            _context.AccessionSections.Add(new AccessionSection
            {
                AccessionSectionId = 1,
                AccessionSectionLineId = 1,
                Acc = "AY874537_3000883",
                PctId = 98.1,
                Aln = 570,
                Glb = 432,
                Len = 861,
                CvgPct = 96,
                Depth = 92.2,
                Cvg = "Oo.ooOOOoOOOOOOOOOOOO.Oo_",
                Fam = "AMR",
                Name = "TEM-11,Proteus mirabilis",
                RunId = 1,
            });
            _context.SaveChanges();
        }
        
        public async Task<IEnumerable<AccessionSection>> GetAccessionSections()
        {
            return await _context.AccessionSections.ToListAsync();
        }

        public async Task<IEnumerable<AccessionSection>> GetRunsFromAccession(string genbank, int page)
        {
            var recordsPerPage = 10;
            if (page == 0)
            {
                page = 1;
            }

            var accs = await _context.AccessionSections
                .Where(a => a.Acc == genbank)
                .OrderByDescending(a => a.CvgPct)
                .Skip((page - 1) * recordsPerPage)
                .Take(recordsPerPage)
                .ToListAsync();

            return accs;
        }

        public async Task<int> GetNumberOfAccessions()
        {
            var accs = await _context.AccessionSections.ToListAsync();
            var numberOfAccs = accs.Count;
            return numberOfAccs;
        }

        public async Task<AccessionSection> GetAccessionSection(int id)
        {
            return await _context.AccessionSections.FindAsync(id);

        }

        public async void PutAccessionSection(int id, AccessionSection accessionSection)
        {
            if (id != accessionSection.AccessionSectionId)
            {
                return;
            }

            _context.Entry(accessionSection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccessionSectionExists(id))
                {
                    return;
                }
                else
                {
                    throw;
                }
            }

            return;
        }

        public async Task<AccessionSection> PostAccessionSection(AccessionSection accessionSection)
        {
            _context.AccessionSections.Add(accessionSection);
            await _context.SaveChangesAsync();

            return accessionSection;
        }

        public async void DeleteAccessionSection(int id)
        {
            var accessionSection = await _context.AccessionSections.FindAsync(id);
            if (accessionSection == null)
            {
                return;
            }

            _context.AccessionSections.Remove(accessionSection);
            await _context.SaveChangesAsync();

            return;
        }

        private bool AccessionSectionExists(int id)
        {
            return _context.AccessionSections.Any(e => e.AccessionSectionId == id);
        }


        // Fasta

        public async Task<IEnumerable<FastaSection>> GetFastaSections()
        {
            return await _context.FastaSections.ToListAsync();
        }

        public async Task<FastaSection> GetFastaSection(int id)
        {
            return await _context.FastaSections.FindAsync(id);
        }

        public async void PutFastaSection(int id, FastaSection fastaSection)
        {
            if (id != fastaSection.FastaSectionId)
            {
                return;
            }

            _context.Entry(fastaSection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FastaSectionExists(id))
                {
                    return;
                }
                else
                {
                    throw;
                }
            }

            return;
        }


        public async Task<FastaSection> PostFastaSection(FastaSection fastaSection)
        {
            _context.FastaSections.Add(fastaSection);
            await _context.SaveChangesAsync();

            return fastaSection;
        }

        public async void DeleteFastaSection(int id)
        {
            var fastaSection = await _context.FastaSections.FindAsync(id);
            if (fastaSection == null)
            {
                return;
            }

            _context.FastaSections.Remove(fastaSection);
            await _context.SaveChangesAsync();

            return;
        }

        private bool FastaSectionExists(int id)
        {
            return _context.FastaSections.Any(e => e.FastaSectionId == id);
        }
    }
}
