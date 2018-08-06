using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Models;
using NotesAPI.Services;

namespace NotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private INotesServices _noteService;

        public NotesController(INotesServices noteService)
        {
            _noteService = noteService;
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
                var note = await _noteService.GetNotes(title, label, isPinned);
                if (note.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(note);
            }
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notes = await _noteService.GetNotesByID(id);

            if (notes == null)
            {
                return NotFound();
            }

            return Ok(notes);
        }


        // PUT: api/Notes/5
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
                await _noteService.PutNotes(notes);
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

            return Ok(notes);
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNotes([FromBody] Notes notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _noteService.PostNotes(notes);

            return CreatedAtAction("GetNotes", new { id = notes.ID }, notes);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notes = await _noteService.DeleteNotes(id);
            if (notes == null)
            {
                return NotFound();
            }

            return Ok(notes);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNotes([FromQuery] string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notes = await _noteService.DeleteNotesByTitle(title);
            if (notes == null)
            {
                return NotFound();
            }

            return Ok(notes);
        }

        private bool NotesExists(int id)
        {
            return _noteService.NotesExist(id);
        }
    }
}