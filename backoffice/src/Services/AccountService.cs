using System.Security.Claims;

namespace Droits.Services;

public interface IAccountService
{
    Guid GetCurrentUserId();
}

public class AccountService : IAccountService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }

        return Guid.Empty;
    }
}