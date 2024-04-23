#region

using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;

#endregion

namespace Droits.Services
{
    public interface INoteService
    {
        Task<Note> SaveNoteAsync(Note note);


        Task SaveFilesAsync(Guid noteId,
            List<DroitFileForm> fileForms);
        Task<Note> GetNoteAsync(Guid id);
        Task<Note> AddNoteAsync(Note note, bool updateLastModified = true);
        Task<bool> DeleteNoteAsync(Guid id);

    }

    public class NoteService : INoteService
    {
        private readonly INoteRepository _repo;
        private readonly IDroitService _droitService;
        private readonly IDroitFileService _fileService;
        private readonly ILogger<NoteService> _logger;



        public NoteService(INoteRepository repo, IDroitService droitService, IDroitFileService fileService, ILogger<NoteService> logger)
        {
            _repo = repo;
            _droitService = droitService;
            _fileService = fileService;
            _logger = logger;

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
        public async Task SaveFilesAsync(Guid noteId,
            List<DroitFileForm> fileForms)
        {
            var fileIdsToKeep = fileForms.Select(f => f.Id);
        
            await _fileService.DeleteDroitFilesForNoteAsync(noteId, fileIdsToKeep);


            try
            {
                var note = await GetNoteAsync(noteId);

                foreach ( var fileForm in fileForms )
                {
                    fileForm.NoteId = note.Id;

                    await _fileService.SaveDroitFileFormAsync(fileForm);
                }

            }
            catch ( NoteNotFoundException e )
            {
                _logger.LogError($"Note not found - {e}");
            }
            catch ( Exception ex )
            {
                _logger.LogError(ex.ToString());
            }
        }
        
        public async Task<Note> AddNoteAsync(Note note, bool updateLastModified = true)
        {
            return await _repo.AddAsync(note, updateLastModified);
        }

        private async Task<Note> UpdateNoteAsync(Note note)
        {
            return await _repo.UpdateAsync(note);
        }

        public async Task<Note> GetNoteAsync(Guid id)
        {
            return await _repo.GetNoteAsync(id);
        }


        public async Task<bool> DeleteNoteAsync(Guid id)
        {
            return await _repo.DeleteNoteAsync(id);

        }
    }
}
