using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SerratusDb.Domain.Model;
using SerratusDb.Services;

namespace SerratusApi.Controllers
{
    [Route("api/genbank")]
    [ApiController]
    public class AccessionSectionsController : ControllerBase
    {
        private readonly ISerratusService _service;

        public AccessionSectionsController(ISerratusService serratusSummaryService)
        {
            _service = serratusSummaryService;
        }

        [HttpPost("create-accession-entry")]
        public void CreateEntry()
        {
            _service.AddAccessionSection();
        }

        // GET: api/genbank
        [HttpGet]
        public async Task<IEnumerable<AccessionSection>> GetAccessionSections()
        {
            return await _service.GetAccessionSections();
        }

        // GET: api/genbank/get-runs/
        [HttpGet("get-runs/{genbank}")]
        public async Task<IEnumerable<AccessionSection>> GetRunsFromAccession(string genbank, [FromQuery] int page)
        {
            return await _service.GetRunsFromAccession(genbank, page);
        }

        // GET: api/genbank/get-number-of-accs
        [HttpGet("get-number-of-accs")]
        public async Task<int> GetNumberOfAccessions()
        {
            var accs = await _service.GetAccessionSections();
            var numberOfAccs = accs.Count();
            return numberOfAccs;    
        }

        // GET: api/AccessionSections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccessionSection>> GetAccessionSection(int id)
        {
            var accessionSection = await _service.GetAccessionSection(id);

            if (accessionSection == null)
            {
                return NotFound();
            }

            return accessionSection;
        }
    }
}
