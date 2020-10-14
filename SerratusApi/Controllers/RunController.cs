using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerratusTest.Domain.Model;
using SerratusTest.ORM;

namespace SerratusApi.Controllers
{
    [Route("api/run")]
    [ApiController]
    public class RunController : ControllerBase
    {
        private readonly SerratusSummaryContext _context;

        public RunController(SerratusSummaryContext context)
        {
            _context = context;
        }

        // GET: api/Runs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Run>>> GetRuns()
        {
            return await _context.run.ToListAsync();
        }
        
        [HttpGet("get-run/{run}")]
        public async Task<ActionResult<Run>> GetSummaryForSra(string run)
        {

            var sra = await _context.run.FirstOrDefaultAsync(r => r.sra_id == run);

            var families = await _context.family
                .Where(f => f.sra_id == sra.sra_id)
                .OrderBy(f => f.family_line)
                .ToListAsync();

            var sequences = await _context.sequence
                .Where(a => a.sra_id == sra.sra_id)
                .OrderBy(a => a.sequence_line)
                .ToListAsync();

            sra.family = families;
            sra.sequence = sequences;

            return sra;
        }

        // GET: api/Runs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Run>> GetRun(int id)
        {
            var run = await _context.run.FindAsync(id);

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
            if (id != run.run_id)
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
            _context.run.Add(run);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRun", new { id = run.run_id }, run);
        }

        // DELETE: api/Runs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Run>> DeleteRun(int id)
        {
            var run = await _context.run.FindAsync(id);
            if (run == null)
            {
                return NotFound();
            }

            _context.run.Remove(run);
            await _context.SaveChangesAsync();

            return run;
        }

        private bool RunExists(int id)
        {
            return _context.run.Any(e => e.run_id == id);
        }
    }
}
