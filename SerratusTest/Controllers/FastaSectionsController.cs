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
    public class FastaSectionsController : ControllerBase
    {
        private readonly SerratusSummaryContext _context;

        public FastaSectionsController(SerratusSummaryContext context)
        {
            _context = context;
        }

        // GET: api/FastaSections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FastaSection>>> GetFastaSections()
        {
            return await _context.FastaSections.ToListAsync();
        }

        // GET: api/FastaSections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FastaSection>> GetFastaSection(int id)
        {
            var fastaSection = await _context.FastaSections.FindAsync(id);

            if (fastaSection == null)
            {
                return NotFound();
            }

            return fastaSection;
        }

        // PUT: api/FastaSections/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFastaSection(int id, FastaSection fastaSection)
        {
            if (id != fastaSection.FastaSectionId)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FastaSections
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FastaSection>> PostFastaSection(FastaSection fastaSection)
        {
            _context.FastaSections.Add(fastaSection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFastaSection", new { id = fastaSection.FastaSectionId }, fastaSection);
        }

        // DELETE: api/FastaSections/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FastaSection>> DeleteFastaSection(int id)
        {
            var fastaSection = await _context.FastaSections.FindAsync(id);
            if (fastaSection == null)
            {
                return NotFound();
            }

            _context.FastaSections.Remove(fastaSection);
            await _context.SaveChangesAsync();

            return fastaSection;
        }

        private bool FastaSectionExists(int id)
        {
            return _context.FastaSections.Any(e => e.FastaSectionId == id);
        }
    }
}
