#region
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

#endregion

namespace Droits.Models.Entities;

[Owned]
public class Address
{
    [JsonProperty("address-line-1")]
    public string? Line1 { get; set; } = string.Empty;

    [JsonProperty("address-line-2")]
    public string? Line2 { get; set; } = string.Empty;

    [JsonProperty("address-town")]
    public string? Town { get; set; } = string.Empty;

    [JsonProperty("address-county")]
    public string? County { get; set; } = string.Empty;

    [JsonProperty("address-postcode")]
    public string? Postcode { get; set; } = string.Empty;
}
