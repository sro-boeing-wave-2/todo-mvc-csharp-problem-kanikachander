using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Models;

namespace NotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesAPIContext _context;

        public NotesController(NotesAPIContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<IActionResult> GetNotes([FromQuery] string title, [FromQuery] string label, [FromQuery] bool? isPinned)
        {
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var note = await _context.Notes.Include(p => p.Label).Include(p => p.CheckedList)
                .Where(p => (p.Title == title || title == null) && (p.Pinned == isPinned || !isPinned.HasValue) && (p.Label.Any(y => y.Label == label) || label == null)).ToListAsync();
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

            try
            {
                //var originalNote = await _context.Notes.Include(p => p.Label).Include(p => p.CheckedList).SingleOrDefaultAsync(p => p.ID == id);
                //if (originalNote != null)
                //{
                //    originalNote.Title = notes.Title;
                //    originalNote.Text = notes.Text;
                //    originalNote.Pinned = notes.Pinned;
                //    for (var i = 0; i < originalNote.Label.Count; i++)
                //    {
                //        originalNote.Label[i].Label = notes.Label[i].Label;
                //    }
                //    for (var i = 0; i < originalNote.CheckedList.Count; i++)
                //    {
                //        originalNote.CheckedList[i].ListItem = notes.CheckedList[i].ListItem;
                //    }
                //}
                //_context.Entry(originalNote).State = EntityState.Modified;
                _context.Notes.Update(notes);
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

            return Ok();
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

        [HttpDelete]
        public async Task<IActionResult> DeleteNotes([FromQuery] string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notes = await _context.Notes.Include(p => p.Label).Include(p => p.CheckedList).Where(p => p.Title == title).ToListAsync();
            if (notes == null)
            {
                return NotFound();
            }
            foreach (var i in notes)
            {
                _context.Notes.Remove(i);
            }

            await _context.SaveChangesAsync();

            return Ok(notes);
        }

        private bool NotesExists(int id)
        {
            return _context.Notes.Any(e => e.ID == id);
        }
    }
}