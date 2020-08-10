using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SerratusDb.Domain.Model;
using SerratusDb.Services;

namespace SerratusApi.Controllers
{
    [Route("api/run")]
    [ApiController]
    public class RunsController : ControllerBase
    {
        private readonly ISerratusService _service;

        public RunsController(ISerratusService serratusSummaryService)
        {
            _service = serratusSummaryService;
        }

        // GET: api/run
        [HttpGet]
        public async Task<IEnumerable<Run>> GetRuns()
        {
            return await _service.GetRuns();
        }

        // GET: api/run/get-run/ERR2756788
        [HttpGet("get-run/{run}")]
        public async Task<Run> GetSummaryForSraAccession(string run)
        {
            return await _service.GetSummaryForSraAccession(run);
        }

        // GET: api/run/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Run>> GetRun(int id)
        {
            var run = await _service.GetRun(id);

            if (run == null)
            {
                return NotFound();
            }

            return run;
        }       
    }
}
