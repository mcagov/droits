using Droits.Models.Entities;
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
        private readonly IDroitService _droitService;

        public NoteService(INoteRepository repo, IDroitService droitService)
        {
            _repo = repo;
            _droitService = droitService;
        }

        public async Task<Note> SaveNoteAsync(Note note)
        {
            if (note.Id == default)
            {
                note = await AddNoteAsync(note);
            }
            else
            {
                note = await UpdateNoteAsync(note);    
            }

            if ( note.DroitId.HasValue && note.DroitId != default )
            {
                var droit = await _droitService.GetDroitAsync(note.DroitId.Value);
                await _droitService.SaveDroitAsync(droit);
            }
            
            return note;
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
