using Microsoft.EntityFrameworkCore;
using Droits.Data;
using Droits.Models.Entities;
using Droits.Services;
using Droits.Exceptions;

namespace Droits.Repositories
{
    public interface INoteRepository
    {
        Task<Note> AddAsync(Note note);
        Task<Note> UpdateAsync(Note note);
        Task<Note> GetNoteAsync(Guid id);
    }

    public class NoteRepository : BaseEntityRepository<Note>, INoteRepository
    {
        public NoteRepository(DroitsContext dbContext, ICurrentUserService currentUserService) : base(dbContext, currentUserService)
        {

        }

        public async Task<Note> GetNoteAsync(Guid id)
        {
            var note = await Context.Notes.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null)
            {
                throw new NoteNotFoundException();
            }
            return note;
        }
    }
}