#region

using Droits.Data;
using Droits.Models.Entities;
using Droits.Services;

#endregion

namespace Droits.Repositories;

public class BaseEntityRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DroitsContext Context;
    private readonly IAccountService _accountService;


    protected BaseEntityRepository(DroitsContext context, IAccountService accountService)
    {
        Context = context;
        _accountService = accountService;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.LastModified = DateTime.UtcNow;
        entity.LastModifiedByUserId = _accountService.GetCurrentUserId();
        
        var savedEntity = Context.Set<TEntity>().Update(entity).Entity;
        await Context.SaveChangesAsync();

        return savedEntity;
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.Created = DateTime.UtcNow;
        entity.LastModified = DateTime.UtcNow;
        entity.LastModifiedByUserId = _accountService.GetCurrentUserId();

        var savedEntity = Context.Set<TEntity>().Add(entity).Entity;
        await Context.SaveChangesAsync();

        return savedEntity;
    }
    
    
    
   
}