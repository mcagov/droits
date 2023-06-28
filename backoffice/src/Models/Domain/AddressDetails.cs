using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Droits.Models;

[Owned]
public class AddressDetails
{
    [JsonProperty("address-line-1")]
    public string Line1 { get; set; } = string.Empty;

    [JsonProperty("address-line-2")]
    public string Line2 { get; set; } = string.Empty;

    [JsonProperty("address-town")]
    public string Town { get; set; } = string.Empty;

    [JsonProperty("address-county")]
    public string County { get; set; } = string.Empty;

    [JsonProperty("address-postcode")]
    public string Postcode { get; set; } = string.Empty;
}