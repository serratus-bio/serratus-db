using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerratusDb.Domain.Model;
using SerratusDb.ORM;

namespace SerratusApi.Controllers
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
    }
}
