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
    public class AccessionSectionsController : ControllerBase
    {
        private readonly SerratusSummaryContext _context;

        public AccessionSectionsController(SerratusSummaryContext context)
        {
            _context = context;
        }

        // GET: api/AccessionSections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccessionSection>>> GetAccessionSections()
        {
            return await _context.AccessionSections.ToListAsync();
        }

        // GET: api/AccessionSections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccessionSection>> GetAccessionSection(int id)
        {
            var accessionSection = await _context.AccessionSections.FindAsync(id);

            if (accessionSection == null)
            {
                return NotFound();
            }

            return accessionSection;
        }

        // PUT: api/AccessionSections/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccessionSection(int id, AccessionSection accessionSection)
        {
            if (id != accessionSection.AccessionSectionId)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AccessionSections
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AccessionSection>> PostAccessionSection(AccessionSection accessionSection)
        {
            _context.AccessionSections.Add(accessionSection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccessionSection", new { id = accessionSection.AccessionSectionId }, accessionSection);
        }

        // DELETE: api/AccessionSections/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AccessionSection>> DeleteAccessionSection(int id)
        {
            var accessionSection = await _context.AccessionSections.FindAsync(id);
            if (accessionSection == null)
            {
                return NotFound();
            }

            _context.AccessionSections.Remove(accessionSection);
            await _context.SaveChangesAsync();

            return accessionSection;
        }

        private bool AccessionSectionExists(int id)
        {
            return _context.AccessionSections.Any(e => e.AccessionSectionId == id);
        }
    }
}
