using System.Security.Claims;
using Droits.Models.Entities;

namespace Droits.Helpers;

public static class RepositoryHelper
{
    public static void UpdateLastModifiedByUserId<TEntity>(TEntity entity)
        where TEntity : BaseEntity
    {
        var userIdClaim = ClaimsPrincipal.Current?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            entity.LastModifiedByUserId = userId;
        }
    }
}