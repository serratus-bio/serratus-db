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
    public class FamilySectionsController : ControllerBase
    {
        private readonly SerratusSummaryContext _context;

        public FamilySectionsController(SerratusSummaryContext context)
        {
            _context = context;
        }

        // GET: api/FamilySections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FamilySection>>> GetFamilySections()
        {
            return await _context.FamilySections.ToListAsync();
        }

        // GET: api/FamilySections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FamilySection>> GetFamilySection(int id)
        {
            var familySection = await _context.FamilySections.FindAsync(id);

            if (familySection == null)
            {
                return NotFound();
            }

            return familySection;
        }

        // PUT: api/FamilySections/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFamilySection(int id, FamilySection familySection)
        {
            if (id != familySection.FamilySectionId)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FamilySections
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FamilySection>> PostFamilySection(FamilySection familySection)
        {
            _context.FamilySections.Add(familySection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFamilySection", new { id = familySection.FamilySectionId }, familySection);
        }

        // DELETE: api/FamilySections/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FamilySection>> DeleteFamilySection(int id)
        {
            var familySection = await _context.FamilySections.FindAsync(id);
            if (familySection == null)
            {
                return NotFound();
            }

            _context.FamilySections.Remove(familySection);
            await _context.SaveChangesAsync();

            return familySection;
        }

        private bool FamilySectionExists(int id)
        {
            return _context.FamilySections.Any(e => e.FamilySectionId == id);
        }
    }
}
