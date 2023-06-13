using Droits.Models;
using Droits.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IEmailRepository
{
    Task<List<Email>> GetEmailsAsync();
    List<Email> GetEmails();
    Task<Email?> GetEmailAsync(Guid id);
    Email AddEmail(Email email);
    void UpdateEmailAsync(Email email);
}

public class EmailRepository : IEmailRepository
{
    private readonly DroitsContext _context;

    public EmailRepository(DroitsContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<List<Email>> GetEmailsAsync()
    {
        return await _context.Emails.ToListAsync();
    }

    public async Task<Email?> GetEmailAsync(Guid id)
    {
        return await _context.Emails.FindAsync(id);
    }
    
    public List<Email> GetEmails()
    {
        return _context.Emails.ToList();
    }

    public Email AddEmail(Email email)
    {
        email.DateCreated = DateTime.UtcNow;

        Email savedEmail = _context.Add(email).Entity;
        _context.SaveChanges();

        return savedEmail;
    }
    
    public async void UpdateEmailAsync(Email email)
    {
        email.DateLastModified = DateTime.UtcNow;

        _context.Emails.Update(email);
        await _context.SaveChangesAsync();
    }
}