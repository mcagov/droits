using Droits.Models;
using Microsoft.EntityFrameworkCore;

namespace Emails.Repositories;

public interface IEmailRepository
{
    Task<List<Email>> GetEmailsAsync();
    Task<Email?> GetEmailAsync(Guid id);
    Task AddEmailAsync(Email email);
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
    
    public async Task AddEmailAsync(Email email)
    {
        email.DateSent = DateTime.UtcNow;

        _context.Emails.Add(email);
        await _context.SaveChangesAsync();
    }
}