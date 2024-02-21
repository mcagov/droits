#region

using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Droits.Repositories;

public interface IUserRepository
{
    IQueryable<ApplicationUser> GetUsers();
    IQueryable<ApplicationUser> GetUsersWithAssociations();
    Task<ApplicationUser> GetUserAsync(Guid id);
    Task<ApplicationUser> GetUserByAuthIdAsync(string authId);
    Task<ApplicationUser> GetUserByEmailAddressAsync(string emailAddress);

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
        return _context.Users.OrderByDescending(u => u.Created); 
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
    
    
    public async Task<ApplicationUser> GetUserByAuthIdAsync(string authId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.AuthId == authId);
        
        if (user == null)
        {
            throw new UserNotFoundException(); 
        }

        return user;
    }
    
    public async Task<ApplicationUser> GetUserByEmailAddressAsync(string emailAddress)
    {
        if ( string.IsNullOrEmpty(emailAddress) )
        {
            throw new UserNotFoundException();
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(emailAddress.ToLower()));
        
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