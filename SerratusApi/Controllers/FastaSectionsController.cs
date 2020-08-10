using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerratusDb.Domain.Model;
using SerratusDb.Services;

namespace SerratusApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FastaSectionsController : ControllerBase
    {
        private readonly ISerratusService _service;

        public FastaSectionsController(ISerratusService serratusSummaryService)
        {
            _service = serratusSummaryService;
        }

        // GET: api/FastaSections
        [HttpGet]
        public async Task<IEnumerable<FastaSection>> GetFastaSections()
        {
            return await _service.GetFastaSections();
        }

        // GET: api/FastaSections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FastaSection>> GetFastaSection(int id)
        {
            var fastaSection = await _service.GetFastaSection(id);

            if (fastaSection == null)
            {
                return NotFound();
            }

            return fastaSection;
        }
    }
}
