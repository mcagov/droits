#region

using System.Security.Claims;
using Droits.Services;
using Microsoft.AspNetCore.Authentication;

#endregion

namespace Droits.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _authenticationScheme;

        public TokenValidationMiddleware(RequestDelegate next, string authenticationScheme)
        {
            _next = next;
            _authenticationScheme = authenticationScheme;
        }

        public async Task InvokeAsync(HttpContext context, ITokenValidationService tokenValidationService)
        {
            var result = await context.AuthenticateAsync(_authenticationScheme);

            if (result is { Succeeded: true, Principal: not null })
            {
                var identity = result.Principal.Identity as ClaimsIdentity;
                await tokenValidationService.OnTokenValidated(identity);
            }

            await _next(context);
        }
    }
}