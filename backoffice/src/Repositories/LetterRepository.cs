using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface ILetterRepository
{
    IQueryable<Letter> GetLetters();
    IQueryable<Letter> GetLettersWithAssociations();
    Task<Letter> GetLetterAsync(Guid id);
    Task<Letter> AddAsync(Letter letter);
    Task<Letter> UpdateAsync(Letter letter);
}

public class LetterRepository : BaseRepository<Letter>, ILetterRepository
{
    public LetterRepository(DroitsContext dbContext): base(dbContext)
    {
    }


    public IQueryable<Letter> GetLetters()
    {
        return Context.Letters.OrderByDescending(l => l.LastModified);
    }


    public IQueryable<Letter> GetLettersWithAssociations()
    {
        return GetLetters().Include(l => l.Droit);
    }


    public async Task<Letter> GetLetterAsync(Guid id)
    {
        var letter = await Context.Letters
                                  .Include(l => l.LastModifiedByUser)
                                  .FirstOrDefaultAsync(l => l.Id == id);

        if ( letter == null )
        {
            throw new LetterNotFoundException();
        }

        return letter;
    }
}
