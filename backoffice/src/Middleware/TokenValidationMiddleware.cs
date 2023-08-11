using System.Security.Claims;
using Droits.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

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
                var identity = result.Principal.Identity as ClaimsIdentity;
                await tokenValidationService.OnTokenValidated(identity);
            }

            await _next(context);
        }
    }
}