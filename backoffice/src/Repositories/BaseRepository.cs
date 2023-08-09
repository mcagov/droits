using Droits.Data;
using Droits.Helpers;
using Droits.Models.Entities;
using Droits.Services;

namespace Droits.Repositories;

public class BaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DroitsContext Context;


    protected BaseRepository(DroitsContext context)
    {
        Context = context;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.LastModified = DateTime.UtcNow;
        RepositoryHelper.UpdateLastModifiedByUserId(entity);
        
        var savedEntity = Context.Set<TEntity>().Update(entity).Entity;
        await Context.SaveChangesAsync();

        return savedEntity;
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.Created = DateTime.UtcNow;
        entity.LastModified = DateTime.UtcNow;
        RepositoryHelper.UpdateLastModifiedByUserId(entity);

        var savedEntity = Context.Set<TEntity>().Add(entity).Entity;
        await Context.SaveChangesAsync();

        return savedEntity;
    }
    
    
    
   
}