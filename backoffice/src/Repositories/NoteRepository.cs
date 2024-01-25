
#region

using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Services;
using Microsoft.EntityFrameworkCore;


#endregion

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
        public NoteRepository(DroitsContext dbContext, IAccountService accountService) : base(dbContext, accountService)
        {

        }

        public async Task<Note> GetNoteAsync(Guid id)
        {
            var note = await Context.Notes.Include(n => n.Files).FirstOrDefaultAsync(n => n.Id == id);
            if (note == null)
            {
                throw new NoteNotFoundException();
            }
            return note;
        }
    }
}