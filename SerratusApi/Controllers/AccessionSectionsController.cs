using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerratusApi.Model;
using SerratusTest.Domain.Model;
using SerratusTest.ORM;
using SerratusTest.Services;

namespace SerratusApi.Controllers
{
    [Route("api/genbank")]
    [ApiController]
    public class AccessionSectionsController : ControllerBase
    {
        private readonly SerratusSummaryContext _context;
        private readonly ISerratusSummaryService _serratusSummaryService;

        public AccessionSectionsController(SerratusSummaryContext context, ISerratusSummaryService serratusSummaryService)
        {
            _context = context;
            _serratusSummaryService = serratusSummaryService;
        }

        [HttpPost("create-accession-entry")]
        public void CreateEntry()
        {
            _serratusSummaryService.AddAccessionSection();
        }

        // GET: api/AccessionSections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccessionSection>>> GetAccessionSections()
        {
            return await _context.AccessionSections.ToListAsync();
        }

        [HttpGet("get-runs/{genbank}")]
        public async Task<ActionResult<PaginatedResult<AccessionSection>>> GetRunsFromAccession(string genbank, [FromQuery] int page, [FromQuery] int itemsPerPage)
        {
            int numPages;
            var totalResults = await _context.AccessionSections
                .Where(a => a.Acc == genbank)
                .OrderByDescending(a => a.CvgPct)
                .CountAsync();

            if (totalResults % itemsPerPage != 0)
            {
                numPages = (totalResults / itemsPerPage) + 1;
            } 
            else
            {
                numPages = totalResults / itemsPerPage;
            }

            var accs = await _context.AccessionSections
                .Where(a => a.Acc == genbank)
                .OrderByDescending(a => a.CvgPct)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();

            var paginatedResult = new PaginatedResult<AccessionSection>
            {
                Items = accs,
                NumberOfPages = numPages
            };
            return paginatedResult;
        }

        [HttpGet("get-number-of-accs")]
        public async Task<int> GetNumberOfAccessions()
        {
            var accs =  await _context.AccessionSections.ToListAsync();
            var numberOfAccs = accs.Count;
            return numberOfAccs;    
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
