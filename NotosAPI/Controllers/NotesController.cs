using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Models;
using NotosAPI.Models;

namespace NotosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotosAPIContext _context;

        public NotesController(NotosAPIContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<IActionResult> GetNotes([FromQuery] string title, [FromQuery] List<string> label, [FromQuery] bool? isPinned)
        {
            if (string.IsNullOrEmpty(title) && !label.Any() && !isPinned.HasValue)
            {
                return Ok(_context.Notes.Include(p => p.Label).Include(p => p.CheckedList));
            }

            else if (!string.IsNullOrEmpty(title) && !label.Any() && !isPinned.HasValue)
            {
                var note = await _context.Notes.Include(p => p.Label).Include(p => p.CheckedList)
                .Where(p => p.Title == title).ToListAsync();
                return Ok(note);
            }

            else if (!string.IsNullOrEmpty(title) && label.Any() && isPinned.HasValue)
            {
                var note = await _context.Notes.Include(p => p.Label).Include(p => p.CheckedList)
                .Where(p => p.Pinned == isPinned).ToListAsync();
            return Ok(note);
            }

            else if (string.IsNullOrEmpty(title) && label.Any() && !isPinned.HasValue)
            {
                var note = await _context.Notes.Include(p => p.Label).Include(p => p.CheckedList)
                .Where(x => x.Label.Any(y => y.Label == label))
                return Ok(note);
            }


        }

        // GET: api/Notes/5
        [Route("{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notes = await _context.Notes.Include(p => p.Label).Include(p => p.CheckedList).SingleOrDefaultAsync(p => p.ID == id);

            if (notes == null)
            {
                return NotFound();
            }

            return Ok(notes);
        }

        //GET: api/Notes? title = "first" [FromQuery(Name = "title")]
        //[Route("SearchByTitle/{title}")]
       
       //[HttpGet("{title}")]
       // public async Task<IActionResult> GetNotesByTitle(string title)
       // {
       //     if (!ModelState.IsValid)
       //     {
       //         return BadRequest(ModelState);
       //     }

       //     var notes = await _context.Notes.Include(p => p.Label).Include(p => p.CheckedList)
       //         .FirstOrDefaultAsync(p => p.Title == title);
       //     //p => p.Title == title
       //     if (notes == null)
       //     {
       //         return NotFound();
       //     }

       //     return Ok(notes);
       // }

        // PUT: api/Notes/5
        //[Route("/{id}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotes([FromRoute] int id, [FromBody] Notes notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notes.ID)
            {
                return BadRequest();
            }

            _context.Entry(notes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotesExists(id))
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

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNotes([FromBody] Notes notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Notes.Add(notes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotes", new { id = notes.ID }, notes);
        }

        // DELETE: api/Notes/5
        //[Route("/{id}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notes = await _context.Notes.Include(p => p.Label).Include(p => p.CheckedList).SingleOrDefaultAsync(p => p.ID == id);
            if (notes == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(notes);
            await _context.SaveChangesAsync();

            return Ok(notes);
        }

        private bool NotesExists(int id)
        {
            return _context.Notes.Any(e => e.ID == id);
        }
    }
}