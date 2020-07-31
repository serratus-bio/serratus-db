using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerratusTest.Domain.Model;
using SerratusTest.ORM;

namespace SerratusTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunsController : ControllerBase
    {
        private readonly SerratusSummaryContext _context;

        public RunsController(SerratusSummaryContext context)
        {
            _context = context;
        }

        // GET: api/Runs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Run>>> GetRuns()
        {
            return await _context.Runs.ToListAsync();
        }
        
        [HttpGet("get-run/{sra}")]
        public async Task<ActionResult<Run>> GetSummary(string sra)
        {

            var run = await _context.Runs.FirstOrDefaultAsync(r => r.Sra == sra);
            var family = await _context.FamilySections
                .Where(f => f.RunId == run.RunId)
                .OrderBy(f => f.FamilySectionLineId)
                .ToListAsync();
            var accs = await _context.AccessionSections
                .Where(a => a.RunId == run.RunId)
                .OrderBy(a => a.AccessionSectionLineId)
                .ToListAsync();
            var fasta = await _context.FastaSections
                .Where(f => f.RunId == run.RunId)
                .OrderBy(f => f.FastaSectionLineId)
                .ToListAsync();
            run.FamilySections = family;
            run.AccessionSections = accs;
            run.FastaSections = fasta;
            return run;
        }

        [HttpGet("get-runs/{accession}")]
        public async Task<ActionResult<IEnumerable<AccessionSection>>> GetRunsFromAccession(string accession, [FromQuery]int page)
        {
            var recordsPerPage = 10;
            if (page == 0)
            {
                page = 1;
            }
            var accs = await _context.AccessionSections
                .Where(a => a.Acc == accession)
                .OrderByDescending(a => a.CvgPct)
                .Skip((page - 1) * recordsPerPage)
                .Take(recordsPerPage)
                .ToListAsync();
            return accs;
        }
        [HttpGet("get-runs-by-family/{family}")]
        public async Task<ActionResult<IEnumerable<FamilySection>>> GetRunsFromFamily(string family)
        {
            var fams = await _context.FamilySections
                .Where(f => f.Family == family)
                .OrderByDescending(f => f.Score)
                .Take(100)
                .ToListAsync();
            return fams;
        }
        // GET: api/Runs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Run>> GetRun(int id)
        {
            var run = await _context.Runs.FindAsync(id);

            if (run == null)
            {
                return NotFound();
            }

            return run;
        }

        // PUT: api/Runs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRun(int id, Run run)
        {
            if (id != run.RunId)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Runs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Run>> PostRun(Run run)
        {
            _context.Runs.Add(run);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRun", new { id = run.RunId }, run);
        }

        // DELETE: api/Runs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Run>> DeleteRun(int id)
        {
            var run = await _context.Runs.FindAsync(id);
            if (run == null)
            {
                return NotFound();
            }

            _context.Runs.Remove(run);
            await _context.SaveChangesAsync();

            return run;
        }

        private bool RunExists(int id)
        {
            return _context.Runs.Any(e => e.RunId == id);
        }
    }
}
