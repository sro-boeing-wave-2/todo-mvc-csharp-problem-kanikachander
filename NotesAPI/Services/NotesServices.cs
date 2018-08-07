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
        Task<Notes> GetNotesByID(int id);
        Task<List<Notes>> GetNotes(string title, string label, bool? isPinned);
        Task<Notes> PostNotes(Notes notes);
        Task<Notes> PutNotes(Notes notes);
        Task<Notes> DeleteNotes(int id);
        Task<List<Notes>> DeleteNotesByTitle(string title);
        bool NotesExist(int id);
    }

    public class NotesServices : INotesServices
    {
        private readonly NotesAPIContext _context;

        public NotesServices(NotesAPIContext context)
        {
            _context = context;
        }

        public Task<Notes> GetNotesByID(int id)
        {
            var note = _context.Notes.Include(p => p.Label).Include(p => p.CheckedList).SingleOrDefaultAsync(p => p.ID == id);
            return note;
        }

        public Task<List<Notes>> GetNotes(string title, string label, bool? isPinned)
        {
            return _context.Notes.Include(p => p.Label).Include(p => p.CheckedList)
                .Where(p => (p.Title == title || title == null) && (p.Pinned == isPinned || !isPinned.HasValue) && (p.Label.Any(y => y.Label == label) || label == null)).ToListAsync();
        }

        public Task<Notes> PostNotes(Notes note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
            return Task.FromResult(note);
        }

        public Task<Notes> PutNotes(Notes note)
        {
            _context.Notes.Update(note);
            _context.SaveChanges();
            return Task.FromResult(note);
        }

        public Task<Notes> DeleteNotes(int id)
        {
            var note = _context.Notes.Include(p => p.Label).Include(p => p.CheckedList).First(p => p.ID == id);
            _context.Notes.Remove(note);
            _context.SaveChanges();
            return Task.FromResult(note);
        }

        public Task<List<Notes>> DeleteNotesByTitle(string title)
        {
            var notes = _context.Notes.Include(p => p.Label).Include(p => p.CheckedList).Where(p => p.Title == title).ToList();
            foreach (var i in notes)
            {
                _context.Notes.Remove(i);
            }
            _context.SaveChanges();
            return Task.FromResult(notes);
        }

        public bool NotesExist(int id)
        {
            return _context.Notes.Any(e => e.ID == id);
        }
    }
}
