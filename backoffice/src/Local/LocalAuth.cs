using System.Security.Claims;

namespace Droits.Local;

public static class LocalAuth
{
    public const string Scheme = "LocalAuth";
    public const string Flag = "DROITS_LOCAL_AUTH";
    public const string ObjectIdClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";

    private static readonly string[] DeployedEnvironmentMarkers =
    {
        "ECS_CONTAINER_METADATA_URI_V4",
        "ECS_CONTAINER_METADATA_URI",
        "ECS_ENABLE_CONTAINER_METADATA"
    };

    public static bool IsRequested(IConfiguration configuration) =>
        string.Equals(configuration[Flag], "true", StringComparison.OrdinalIgnoreCase);

    public static bool IsAllowed(IConfiguration configuration, IHostEnvironment environment) =>
        environment.IsDevelopment() &&
        DeployedEnvironmentMarkers.All(marker => string.IsNullOrEmpty(configuration[marker]));

    public static bool IsEnabled(IConfiguration configuration, IHostEnvironment environment) =>
        IsRequested(configuration) && IsAllowed(configuration, environment);

    public static string Email(IConfiguration configuration) =>
        configuration["LOCAL_AUTH_EMAIL"] ?? "dev@droits.local";

    public static IEnumerable<Claim> Claims(IConfiguration configuration) => new[]
    {
        new Claim(ObjectIdClaimType, configuration["LOCAL_AUTH_ID"] ?? "00000000-0000-4000-8000-000000000001"),
        new Claim("name", configuration["LOCAL_AUTH_NAME"] ?? "Dev User"),
        new Claim("preferred_username", Email(configuration)),
        new Claim("groups", configuration["AzureAd:GroupId"] ?? string.Empty)
    };
}
