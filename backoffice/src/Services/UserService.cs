using Droits.Helpers;
using Droits.Models.Entities;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Droits.Services;

public interface IUserService
{
    Task<UserListView> GetUserListViewAsync(SearchOptions searchOptions);
    Task<List<ApplicationUser>> GetUsersAsync();
    Task<ApplicationUser> SaveUserAsync(ApplicationUser user);
    Task<ApplicationUser> GetUserAsync(Guid id);
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
}