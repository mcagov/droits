
namespace Droits.Helpers;

public static class RequestHelper
{
    public static bool IsValidApiKey(string apiKey, IConfiguration configuration)
    {
        var validApiKey = configuration["ApiKey"];
        return !string.IsNullOrEmpty(validApiKey) && apiKey == validApiKey;
    }
}