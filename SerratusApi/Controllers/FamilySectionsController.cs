using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SerratusDb.Domain.Model;
using SerratusDb.Services;

namespace SerratusApi.Controllers
{
    [Route("api/family")]
    [ApiController]
    public class FamilySectionsController : ControllerBase
    {
        private readonly ISerratusSummaryService _serratusSummaryService;

        public FamilySectionsController(SerratusSummaryService serratusSummaryService)
        {
            _serratusSummaryService = serratusSummaryService;
        }

        // POST
        [HttpPost("create-family-section")]
        public void CreateEntry()
        {
            _serratusSummaryService.AddFamilySection();
        }

        // GET: api/family
        [HttpGet]
        public async Task<IEnumerable<FamilySection>> GetFamilySections()
        {
            return await _serratusSummaryService.GetFamilySections();
        }

        // GET: api/family/get-runs/Coronaviridae
        [HttpGet("get-runs/{family}")]
        public async Task<IEnumerable<FamilySection>> GetRunsFromFamily(string family, [FromQuery] int page)
        {
            return await _serratusSummaryService.GetRunsFromFamily(family, page);
        }

        // GET: api/family/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FamilySection>> GetFamilySection(int id)
        {
            var familySection = await _serratusSummaryService.GetFamilySection(id);

            if (familySection == null)
            {
                return NotFound();
            }

            return familySection;
        }
    }
}
