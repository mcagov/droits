using System.Security.Claims;

namespace Droits.Services;
public interface ITokenValidationService
{
    Task OnTokenValidated(ClaimsIdentity? identity);
}

public class TokenValidationService : ITokenValidationService
{
    private readonly IUserService _userService;

    public TokenValidationService(IUserService userService)
    {
        _userService = userService;
    }

  public async Task OnTokenValidated(ClaimsIdentity? identity)
{
    try
    {
        if (identity == null)
        {
            throw new Exception("Unable to get the ClaimsIdentity from the Principal.");
        }

        var objectIdClaim = identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier");
        if (objectIdClaim == null)
        {
            throw new Exception("ObjectIdentifier claim not found.");
        }
        var objectId = objectIdClaim.Value;
        
        var nameClaim = identity.FindFirst("name");
        var name = nameClaim?.Value ?? string.Empty;

        var emailClaim = identity.FindFirst("preferred_username");
        var email = emailClaim?.Value ?? string.Empty;

        var applicationUser = await _userService.GetOrCreateUserAsync(objectId, name, email);

        var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || userIdClaim.Value != applicationUser.Id.ToString())
        {
            if (userIdClaim != null)
            {
                identity.RemoveClaim(userIdClaim);
            }
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id.ToString()));
        }

        var userEmailClaim = identity.FindFirst(ClaimTypes.Email);
        if (userEmailClaim == null || userEmailClaim.Value != applicationUser.Email)
        {
            if (userEmailClaim != null)
            {
                identity.RemoveClaim(userEmailClaim);
            }
            identity.AddClaim(new Claim(ClaimTypes.Email, applicationUser.Email));
        }
    }
    catch (Exception ex)
    {
        throw new Exception("Error during token validation.", ex);
    }
}




}
