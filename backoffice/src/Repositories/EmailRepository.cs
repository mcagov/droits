using Droits.Exceptions;
using Droits.Models;
using Droits.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IEmailRepository
{
    Task<List<Email>> GetEmailsAsync();
    Task<Email> GetEmailAsync(Guid id);
    Task<Email> AddEmailAsync(Email email);
    Task<Email> UpdateEmailAsync(Email email);
}

public class EmailRepository : IEmailRepository
{
    private readonly DroitsContext _context;

    public EmailRepository(DroitsContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<List<Email>> GetEmailsAsync() =>
        await _context.Emails.ToListAsync();


    public async Task<Email> GetEmailAsync(Guid id)
    {
        var email = await _context.Emails.FindAsync(id);
        if (email != null)
        {
            return email;
        }

        throw new EmailNotFoundException();
    } 


    public List<Email> GetEmails()
    {
        return _context.Emails.ToList();
    }

    public async Task<Email> AddEmailAsync(Email email)
    {
        email.DateCreated = DateTime.UtcNow;
        email.DateLastModified = DateTime.UtcNow;
        
        var savedEmail = _context.Add(email).Entity;
        await _context.SaveChangesAsync();

        return savedEmail;
    }
    
    public async Task<Email> UpdateEmailAsync(Email email)
    {
        email.DateLastModified = DateTime.UtcNow;

        _context.Emails.Update(email);
        
        await _context.SaveChangesAsync();

        return email;
    }
}