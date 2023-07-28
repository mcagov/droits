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
    Task<Letter> AddLetterAsync(Letter letter);
    Task<Letter> UpdateLetterAsync(Letter letter);
}

public class LetterRepository : ILetterRepository
{
    private readonly DroitsContext _context;


    public LetterRepository(DroitsContext dbContext)
    {
        _context = dbContext;
    }


    public IQueryable<Letter> GetLetters()
    {
        return _context.Letters;
    }
    
    public IQueryable<Letter> GetLettersWithAssociations()
    {
        return GetLetters();
    }

    public async Task<Letter> GetLetterAsync(Guid id)
    {
        var letter = await _context.Letters.FindAsync(id);

        if ( letter == null )
        {
            throw new LetterNotFoundException();
        }

        return letter;
    }
    
    public async Task<Letter> AddLetterAsync(Letter letter)
    {
        letter.Created = DateTime.UtcNow;
        letter.LastModified = DateTime.UtcNow;

        var savedLetter = _context.Add(letter).Entity;
        await _context.SaveChangesAsync();

        return savedLetter;
    }


    public async Task<Letter> UpdateLetterAsync(Letter letter)
    {
        letter.LastModified = DateTime.UtcNow;

        _context.Letters.Update(letter);

        await _context.SaveChangesAsync();

        return letter;
    }
    
}