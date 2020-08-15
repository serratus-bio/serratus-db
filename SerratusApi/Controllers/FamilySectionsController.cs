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
    [Route("api/family")]

    [ApiController]
    public class FamilySectionsController : ControllerBase
    {
        private readonly SerratusSummaryContext _context;
        private readonly ISerratusSummaryService _serratusSummaryService;

        public FamilySectionsController(SerratusSummaryContext context, ISerratusSummaryService serratusSummaryService)
        {
            _context = context;
            _serratusSummaryService = serratusSummaryService;
        }

        [HttpPost("create-family-section")]
        public void CreateEntry()
        {
            _serratusSummaryService.AddFamilySection();
        }

        // GET: api/FamilySections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FamilySection>>> GetFamilySections()
        {
            return await _context.FamilySections.ToListAsync();
        }

        [HttpGet("get-runs/{family}")]
        public async Task<ActionResult<PaginatedResult<FamilySection>>> GetRunsFromFamily(string family,
            [FromQuery] int page,
            [FromQuery] int itemsPerPage,
            [FromQuery] string pctId, // [int-int]
            [FromQuery] string score)
        {
            try
            {
                (int low, int high) pctIdLimits = Utilities.parseQueryParameterRange(pctId);
                (int low, int high) scoreLimits = Utilities.parseQueryParameterRange(score);

                int numPages;
                var totalResults = await _context.FamilySections
                    .Where(f => f.Family == family)
                    .OrderByDescending(f => f.Score)
                    .CountAsync();

                if (totalResults % itemsPerPage != 0)
                {
                    numPages = (totalResults / itemsPerPage) + 1;
                }
                else
                {
                    numPages = totalResults / itemsPerPage;
                }

                var families = await _context.FamilySections
                    .Where(f => f.Family == family
                        && (f.PctId > pctIdLimits.low && f.PctId < pctIdLimits.high)
                        && (f.Score > scoreLimits.low && f.Score < scoreLimits.high)
                    )
                    .OrderByDescending(f => f.Score)
                    .Skip((page - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToListAsync();

                var paginatedResult = new PaginatedResult<FamilySection>
                {
                    Items = families,
                    NumberOfPages = numPages
                };
                return paginatedResult;
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
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
