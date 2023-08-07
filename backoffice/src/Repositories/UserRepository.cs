using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IUserRepository
{
    IQueryable<ApplicationUser> GetUsers();
    IQueryable<ApplicationUser> GetUsersWithAssociations();
    Task<ApplicationUser> GetUserAsync(Guid id);
    Task<ApplicationUser> AddUserAsync(ApplicationUser user);
    Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
}

public class UserRepository : IUserRepository
{
    private readonly DroitsContext _context;

    public UserRepository(DroitsContext dbContext)
    {
        _context = dbContext;
    }

    public IQueryable<ApplicationUser> GetUsers()
    {
        return _context.Users.OrderByDescending(u => u.LastModified); 
    }

    public IQueryable<ApplicationUser> GetUsersWithAssociations()
    {
        return GetUsers();
    }

    public async Task<ApplicationUser> GetUserAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
        {
            throw new UserNotFoundException(); 
        }

        return user;
    }

    public async Task<ApplicationUser> AddUserAsync(ApplicationUser user)
    {
        user.Created = DateTime.UtcNow; 
        user.LastModified = DateTime.UtcNow;

        var savedUser = _context.Users.Add(user).Entity;
        await _context.SaveChangesAsync();

        return savedUser;
    }

    public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
    {
        user.LastModified = DateTime.UtcNow; 

        var savedUser = _context.Users.Update(user).Entity;
        await _context.SaveChangesAsync();

        return savedUser;
    }
}