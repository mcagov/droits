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

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool updateLastModified = true)
    {

        if ( updateLastModified )
        {
            entity.LastModified = DateTime.UtcNow;
            entity.LastModifiedByUserId = _accountService.GetCurrentUserId(); 
        }

        var savedEntity = Context.Set<TEntity>().Update(entity).Entity;
        await Context.SaveChangesAsync();

        return savedEntity;
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, bool updateLastModified = true)
    {

        if ( updateLastModified )
        {
            entity.LastModified = DateTime.UtcNow;
            entity.LastModifiedByUserId = _accountService.GetCurrentUserId();
        }

        var savedEntity = Context.Set<TEntity>().Add(entity).Entity;
        await Context.SaveChangesAsync();

        return savedEntity;
    }
    
    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        if (entity.Id == default)
        {
            return false; 
        }

        // Define allowed TEntity types
        var allowedEntityTypes = new List<Type>
        {
            typeof(Note),
        };

        if (!allowedEntityTypes.Contains(entity.GetType()))
        {
            throw new InvalidOperationException("This entity type is not allowed to be deleted.");
        }
        
        Context.Set<TEntity>().Remove(entity);
        await Context.SaveChangesAsync();

        return true;
    }
    
    
    
    
   
}