using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface ILetterRepository
{
    Task<List<Letter>> GetLettersAsync();
    Task<Letter> GetLetterAsync(Guid id);
    Task<Letter> AddLetterAsync(Letter letter);
    Task<Letter> UpdateLetterAsync(Letter letter);
    Task<List<Letter>> GetLettersForRecipientAsync(string recipient);
}

public class LetterRepository : ILetterRepository
{
    private readonly DroitsContext _context;


    public LetterRepository(DroitsContext dbContext)
    {
        _context = dbContext;
    }


    public async Task<List<Letter>> GetLettersAsync()
    {
        return await _context.Letters.ToListAsync();
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


    public List<Letter> GetLetters()
    {
        return _context.Letters.ToList();
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


    public async Task<List<Letter>> GetLettersForRecipientAsync(string recipient)
    {
        return await _context.Letters
            .Where(e => e.Recipient.Equals(recipient))
            .OrderByDescending(e => e.LastModified)
            .ToListAsync();
    }
}