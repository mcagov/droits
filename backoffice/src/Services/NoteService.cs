using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;

namespace Droits.Services
{
    public interface INoteService
    {
        Task<Note> SaveNoteAsync(Note note);
        Task<Note> GetNoteAsync(Guid id);
    }

    public class NoteService : INoteService
    {
        private readonly INoteRepository _repo;

        public NoteService(INoteRepository repo)
        {
            _repo = repo;
        }

        public async Task<Note> SaveNoteAsync(Note note)
        {
            if (note.Id == default)
            {
                return await AddNoteAsync(note);
            }
            return await UpdateNoteAsync(note);
        }

        private async Task<Note> AddNoteAsync(Note note)
        {
            return await _repo.AddAsync(note);
        }

        private async Task<Note> UpdateNoteAsync(Note note)
        {
            return await _repo.UpdateAsync(note);
        }

        public async Task<Note> GetNoteAsync(Guid id)
        {
            return await _repo.GetNoteAsync(id);
        }

    }
}
