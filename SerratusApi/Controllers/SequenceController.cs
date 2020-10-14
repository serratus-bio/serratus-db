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
using Utilities = SerratusApi.Utills.Utills;

namespace SerratusApi.Controllers
{
    [Route("api/genbank")]
    [ApiController]
    public class SequenceController : ControllerBase
    {
        private readonly SerratusSummaryContext _context;
        private readonly ISerratusSummaryService _serratusSummaryService;

        public SequenceController(SerratusSummaryContext context, ISerratusSummaryService serratusSummaryService)
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
        public async Task<ActionResult<IEnumerable<Sequence>>> GetAccessionSections()
        {
            return await _context.sequence.ToListAsync();
        }

        [HttpGet("get-runs/{genbank}")]
        public async Task<ActionResult<PaginatedResult<Sequence>>> GetRunsFromAccession(string genbank,
            [FromQuery] int page,
            [FromQuery] int itemsPerPage,
            [FromQuery] string? pctId, // [int-int]
            [FromQuery] string? cvgPct)
        {
            try
            {
                var items = _context.sequence.Where(a => a.genbank_name == genbank);
                if (pctId != null)
                {
                    (int low, int high) pctIdLimits = Utilities.parseQueryParameterRange(pctId);
                    items = items.Where(a => a.percentage_identity >= pctIdLimits.low && a.percentage_identity <= pctIdLimits.high);
                }
                if (cvgPct != null)
                {
                    (int low, int high) scoreLimits = Utilities.parseQueryParameterRange(cvgPct);
                    items = items.Where(a => a.percentage_identity >= scoreLimits.low && a.percentage_identity <= scoreLimits.high);
                }
                var sequences = await items.OrderByDescending(a => a.percentage_identity)
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage).ToListAsync();
                int numPages;
                var totalResults = await items.CountAsync();

                if (totalResults % itemsPerPage != 0)
                {
                    numPages = (totalResults / itemsPerPage) + 1;
                }
                else
                {
                    numPages = totalResults / itemsPerPage;
                }

                var paginatedResult = new PaginatedResult<Sequence>
                {
                    Items = sequences,
                    NumberOfPages = numPages
                };
                return paginatedResult;
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        [HttpGet("get-number-of-accs")]
        public async Task<int> GetNumberOfAccessions()
        {
            var sequences =  await _context.sequence.ToListAsync();
            var numberOfAccs = sequences.Count;
            return numberOfAccs;    
        }

        // GET: api/AccessionSections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sequence>> GetAccessionSection(int id)
        {
            var accessionSection = await _context.sequence.FindAsync(id);

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
        public async Task<IActionResult> PutAccessionSection(int id, Sequence sequence)
        {
            if (id != sequence.sequence_id)
            {
                return BadRequest();
            }

            _context.Entry(sequence).State = EntityState.Modified;

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
        public async Task<ActionResult<Sequence>> PostAccessionSection(Sequence sequence)
        {
            _context.sequence.Add(sequence);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccessionSection", new { id = sequence.sequence_id}, sequence);
        }

        // DELETE: api/AccessionSections/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Sequence>> DeleteAccessionSection(int id)
        {
            var accessionSection = await _context.sequence.FindAsync(id);
            if (accessionSection == null)
            {
                return NotFound();
            }

            _context.sequence.Remove(accessionSection);
            await _context.SaveChangesAsync();

            return accessionSection;
        }

        private bool AccessionSectionExists(int id)
        {
            return _context.sequence.Any(e => e.sequence_id == id);
        }
    }
}
