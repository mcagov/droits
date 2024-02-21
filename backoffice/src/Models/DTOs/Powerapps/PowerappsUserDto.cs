using System.Text.Json.Serialization;
using Droits.Helpers;

namespace Droits.Models.DTOs.Powerapps
{
    public class PowerappsUserDto
    {
        [JsonPropertyName("internalemailaddress")]
        public string? EmailAddress { get; set; }

        [JsonPropertyName("fullname")]
        public string? FullName { get; set; }
    }
}
