#region

using System.Security.Claims;
using Droits.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

#endregion

namespace Droits.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITokenValidationService tokenValidationService)
        {
            var result = await context.AuthenticateAsync(OpenIdConnectDefaults.AuthenticationScheme);

            if (result is { Succeeded: true, Principal: not null })
            {
                var principal = result.Principal;
                var identity = principal.Identity as ClaimsIdentity;

                var adGroupId = context.RequestServices.GetRequiredService<IConfiguration>().GetSection("AzureAd:GroupId").Value ?? "";

                // Filter out and remove 'groups' claims where the groupId isn't equal to adGroupId
                var filteredClaims = identity?.Claims.Where(c => c.Type != "groups" || c.Value == adGroupId).ToList();

                // Clear existing claims and add filtered claims back to the identity
                identity?.Claims.ToList().ForEach(c => identity.RemoveClaim(c));
                filteredClaims?.ForEach(c => identity?.AddClaim(c));
            }

            await _next(context);
        }
    }
}