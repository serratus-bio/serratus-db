﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerratusTest.Domain.Model;
using SerratusTest.ORM;
using SerratusTest.Services;

namespace SerratusTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentLinesController : ControllerBase
    {
        private readonly SerratusSummaryContext _context;
        private readonly ISerratusSummaryService _serratusSummaryService;

        public CommentLinesController(SerratusSummaryContext context, ISerratusSummaryService serratusSummaryService)
        {
            _context = context;
            _serratusSummaryService = serratusSummaryService;
        }

        [HttpPost("create-summary")]
        public void CreateEntry()
        {
            _serratusSummaryService.AddCommentLine();
        }
        // GET: api/CommentLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Run>>> GetCommentLines()
        {
            return await _context.CommentLines.ToListAsync();
        }

        // GET: api/CommentLines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Run>> GetCommentLine(int id)
        {
            var commentLine = await _context.CommentLines.FindAsync(id);

            if (commentLine == null)
            {
                return NotFound();
            }

            return commentLine;
        }

        // PUT: api/CommentLines/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommentLine(int id, Run commentLine)
        {
            if (id != commentLine.CommentLineId)
            {
                return BadRequest();
            }

            _context.Entry(commentLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentLineExists(id))
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

        // POST: api/CommentLines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Run>> PostCommentLine(Run commentLine)
        {
            _context.CommentLines.Add(commentLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommentLine", new { id = commentLine.CommentLineId }, commentLine);
        }

        // DELETE: api/CommentLines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Run>> DeleteCommentLine(int id)
        {
            var commentLine = await _context.CommentLines.FindAsync(id);
            if (commentLine == null)
            {
                return NotFound();
            }

            _context.CommentLines.Remove(commentLine);
            await _context.SaveChangesAsync();

            return commentLine;
        }

        private bool CommentLineExists(int id)
        {
            return _context.CommentLines.Any(e => e.CommentLineId == id);
        }
    }
}
