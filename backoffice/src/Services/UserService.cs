using Droits.Helpers;
using Droits.Models.Entities;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Droits.Exceptions;

namespace Droits.Services;

public interface IUserService
{
    Task<UserListView> GetUserListViewAsync(SearchOptions searchOptions);
    Task<List<ApplicationUser>> GetUsersAsync();
    Task<ApplicationUser> SaveUserAsync(ApplicationUser user);
    Task<ApplicationUser> GetUserAsync(Guid id);
    Task<ApplicationUser> GetOrCreateUserAsync(string authId, string name, string email);
}

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<UserListView> GetUserListViewAsync(SearchOptions searchOptions)
    {
        var query = searchOptions.IncludeAssociations
            ? _repo.GetUsersWithAssociations()
            : _repo.GetUsers();
        var pagedItems = await ServiceHelpers.GetPagedResult(
            query.Select(u => new UserView(u, searchOptions.IncludeAssociations)), searchOptions);

        return new UserListView(pagedItems.Items)
        {
            PageNumber = pagedItems.PageNumber,
            PageSize = pagedItems.PageSize,
            IncludeAssociations = pagedItems.IncludeAssociations,
            TotalCount = pagedItems.TotalCount
        };
    }

    public async Task<List<ApplicationUser>> GetUsersAsync()
    {
        return await _repo.GetUsers().ToListAsync();
    }

    public async Task<ApplicationUser> SaveUserAsync(ApplicationUser user)
    {
        if (user.Id == Guid.Empty)
        {
            return await _repo.AddUserAsync(user);
        }
        return await _repo.UpdateUserAsync(user);
    }

    public async Task<ApplicationUser> GetUserAsync(Guid id)
    {
        return await _repo.GetUserAsync(id);
    }
    
    public async Task<ApplicationUser> GetOrCreateUserAsync(string authId, string name, string email)
    {
        try
        {
            return await _repo.GetUserByAuthIdAsync(authId);
        }
        catch ( UserNotFoundException )
        {
            var newUser = new ApplicationUser
            {
                AuthId = authId,
                Name = name,
                Email = email,
            };

            return await _repo.AddUserAsync(newUser);    
        }
    }
}