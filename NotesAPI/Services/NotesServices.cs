using Microsoft.EntityFrameworkCore;
using NotesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesAPI.Services
{
    public interface INotesServices
    {
        Task<Note> GetNotesByID(int id);
        Task<List<Note>> GetNotes(string title, string label, bool? isPinned);
        Task<Note> PostNotes(Note note);
        Task<Note> PutNotes(Note note);
        Task<Note> DeleteNotes(int id);
        Task<List<Note>> DeleteNotesByTitle(string title);
        bool NotesExist(int id);
    }

    public class NotesServices : INotesServices
    {
        private readonly NotesAPIContext _context;

        public NotesServices(NotesAPIContext context)
        {
            _context = context;
        }

        public async Task<Note> GetNotesByID(int id)
        {
            var note = await _context.Note.Include(p => p.Labels).Include(p => p.CheckedList).SingleOrDefaultAsync(p => p.ID == id);
            return note;
        }

        public async Task<List<Note>> GetNotes(string title, string label, bool? isPinned)
        {
            Func<Note, bool> NoteMatchesTitleOrLabelOrIsPinned = (p) =>
            (p.Title == title || String.IsNullOrEmpty(title) 
                && (p.Pinned == isPinned || !isPinned.HasValue)
                && (p.Labels.Any(y => y.LabelName == label) || String.IsNullOrEmpty(label)));

            var x = _context.Note.Include(p => p.Labels)
                .Include(p => p.CheckedList)
                .Where(NoteMatchesTitleOrLabelOrIsPinned).ToList();
            return await Task.FromResult(x);
        }

        public async Task<Note> PostNotes(Note note)
        {
            _context.Note.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> PutNotes(Note note)
        {
            _context.Note.Update(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> DeleteNotes(int id)
        {
            var note = _context.Note.Include(p => p.Labels).Include(p => p.CheckedList).SingleOrDefault(p => p.ID == id);
            if(note == null)
            {
                return note;
            }
            _context.Note.Remove(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<List<Note>> DeleteNotesByTitle(string title)
        {
            var notes = _context.Note.Include(p => p.Labels).Include(p => p.CheckedList).Where(p => p.Title == title).ToList();
            if (notes == null)
            {
                return notes;
            }
            foreach (var i in notes)
            {
                _context.Note.Remove(i);
            }
            await _context.SaveChangesAsync();
            return notes;
        }

        public bool NotesExist(int id)
        {
            return _context.Note.Any(e => e.ID == id);
        }
    }
}
