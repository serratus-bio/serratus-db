using System.Collections.Generic;
using System.Threading.Tasks;
using SerratusDb.Domain.Model;

namespace SerratusDb.Services
{
    public interface ISerratusSummaryService
    {
        // Runs
        void AddRun();

        Task<IEnumerable<Run>> GetRuns();

        Task<Run> GetSummaryForSraAccession(string run);

        Task<Run> GetRun(int id);

        void PutRun(int id, Run run);

        Task<Run> PostRun(Run run);

        void DeleteRun(int id);

        // Family
        void AddFamilySection();

        Task<IEnumerable<FamilySection>> GetRunsFromFamily(string family);

        Task<IEnumerable<FamilySection>> GetFamilySections();

        Task<FamilySection> GetFamilySection(int id);

        void PutFamilySection(int id, FamilySection familySection);

        Task<FamilySection> PostFamilySection(FamilySection familySection);

        void DeleteFamilySection(int id);

        // Accession

        void AddAccessionSection();

        Task<IEnumerable<AccessionSection>> GetAccessionSections();

        Task<IEnumerable<AccessionSection>> GetRunsFromAccession(string genbank, int page);

        Task<int> GetNumberOfAccessions();

        Task<AccessionSection> GetAccessionSection(int id);

        void PutAccessionSection(int id, AccessionSection accessionSection);

        Task<AccessionSection> PostAccessionSection(AccessionSection accessionSection);

        void DeleteAccessionSection(int id);

        // Fasta

        Task<IEnumerable<FastaSection>> GetFastaSections();

        Task<FastaSection> GetFastaSection(int id);

        void PutFastaSection(int id, FastaSection fastaSection);

        Task<FastaSection> PostFastaSection(FastaSection fastaSection);

        void DeleteFastaSection(int id);
    }
}