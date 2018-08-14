using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
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
        Task<Note> PutNotes(int id, Note note);
        Task<Note> DeleteNotes(int id);
        Task<List<Note>> DeleteNotesByTitle(string title);
        bool NotesExist(int id);
    }

    public class NotesServices : INotesServices
    {
        private readonly NotesAPIContext _context;

        public NotesServices(IOptions<Settings> settings)
        {
            _context = new NotesAPIContext(settings);
        }

        public async Task<Note> GetNotesByID(int id)
        {
            var note = await _context.Note.Find(p => p.ID == id).FirstOrDefaultAsync();
            return note;
        }

        public async Task<List<Note>> GetNotes(string title, string label, bool? isPinned)
        {
            Func<Note, bool> NoteMatchesTitleOrLabelOrIsPinned = (p) =>
            (p.Title == title || String.IsNullOrEmpty(title) 
                && (p.Pinned == isPinned || !isPinned.HasValue)
                && (p.Labels.Any(y => y.LabelName == label) || String.IsNullOrEmpty(label)));

            var x = _context.Note.AsQueryable<Note>().Where(NoteMatchesTitleOrLabelOrIsPinned).ToList();
            return await Task.FromResult(x);
        }

        public async Task<Note> PostNotes(Note note)
        {
            await _context.Note.InsertOneAsync(note);
            //await _context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> PutNotes(int id, Note note)
        {
            var filter = Builders<Note>.Filter.Eq(p => p.ID,id);
            var update = Builders<Note>.Update.Set(p => p.Title, note.Title)
                .Set(p => p.Text, note.Text)
                .Set(p => p.Labels, note.Labels)
                .Set(p => p.Pinned, note.Pinned)
                .Set(p => p.CheckedList, note.CheckedList);
            await _context.Note.UpdateOneAsync(filter, update);
            //await _context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> DeleteNotes(int id)
        {
            var note = await _context.Note.Find(p => p.ID == id).FirstOrDefaultAsync();
            if (note == null)
            {
                return note;
            }
            DeleteResult deleteResult = await _context.Note.DeleteOneAsync(Builders<Note>.Filter.Eq(p => p.ID, id));
            
            return note;
        }

        public async Task<List<Note>> DeleteNotesByTitle(string title)
        {
            var notes = await _context.Note.Find(p => p.Title == title).ToListAsync();
            if (notes == null)
            {
                return notes;
            }
            DeleteResult deleteResult = await _context.Note.DeleteManyAsync(Builders<Note>.Filter.Eq(p => p.Title, title));
            //await _context.SaveChangesAsync();
            return notes;
        }

        public bool NotesExist(int id)
        {
            return _context.Note.AsQueryable().Any(e => e.ID == id);
        }
    }
}
