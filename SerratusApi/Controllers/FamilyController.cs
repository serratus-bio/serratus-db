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
    public class FamilyController : ControllerBase
    {
        private readonly SerratusSummaryContext _context;
        private readonly ISerratusSummaryService _serratusSummaryService;

        public FamilyController(SerratusSummaryContext context, ISerratusSummaryService serratusSummaryService)
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
        public async Task<ActionResult<IEnumerable<Family>>> GetFamilySections()
        {
            return await _context.family.ToListAsync();
        }

        [HttpGet("get-runs/{family}")]
        public async Task<ActionResult<PaginatedResult<Family>>> GetRunsFromFamily(string family,
            [FromQuery] int page,
            [FromQuery] int itemsPerPage,
            [FromQuery] string? pctId, // [int-int]
            [FromQuery] string? score)
        {
            try
            {
                var items = _context.family.Where(f => f.family_name == family);
                if (pctId != null)
                {
                    (int low, int high) pctIdLimits = Utilities.parseQueryParameterRange(pctId);
                    items = items.Where(f => f.percent_identity >= pctIdLimits.low && f.percent_identity<= pctIdLimits.high);
                }
                if (score != null)
                {
                    (int low, int high) scoreLimits = Utilities.parseQueryParameterRange(score);
                    items = items.Where(f => f.score >= scoreLimits.low && f.score <= scoreLimits.high);
                }
                var families = await items.OrderByDescending(f => f.score)
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

                var paginatedResult = new PaginatedResult<Family>
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
        public async Task<ActionResult<Family>> GetFamilySection(int id)
        {
            var familySection = await _context.family.FindAsync(id);

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
        public async Task<IActionResult> PutFamilySection(int id, Family family)
        {
            if (id != family.family_id)
            {
                return BadRequest();
            }

            _context.Entry(family).State = EntityState.Modified;

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
        public async Task<ActionResult<Family>> PostFamilySection(Family familySection)
        {
            _context.family.Add(familySection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFamilySection", new { id = familySection.family_id }, familySection);
        }

        // DELETE: api/FamilySections/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Family>> DeleteFamilySection(int id)
        {
            var familySection = await _context.family.FindAsync(id);
            if (familySection == null)
            {
                return NotFound();
            }

            _context.family.Remove(familySection);
            await _context.SaveChangesAsync();

            return familySection;
        }

        private bool FamilySectionExists(int id)
        {
            return _context.family.Any(e => e.family_id == id);
        }
    }
}
