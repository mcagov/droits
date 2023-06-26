using Newtonsoft.Json;

namespace Droits.Models;

public class WreckReport
{
    public string Reference { get; set; } = string.Empty;

    [JsonProperty("report-date")]
    public string ReportDate { get; set; } = string.Empty;

    [JsonProperty("wreck-find-date")]
    public string WreckFindDate { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    [JsonProperty("location-radius")]
    public int LocationRadius { get; set; }

    [JsonProperty("location-description")]
    public string LocationDescription { get; set; } = string.Empty;

    [JsonProperty("vessel-name")]
    public string VesselName { get; set; } = string.Empty;

    [JsonProperty("vessel-construction-year")]
    public string VesselConstructionYear { get; set; } = string.Empty;

    [JsonProperty("vessel-sunk-year")]
    public string VesselSunkYear { get; set; } = string.Empty;

    [JsonProperty("vessel-depth")]
    public string VesselDepth { get; set; } = string.Empty;

    [JsonProperty("removed-from")]
    public string RemovedFrom { get; set; } = string.Empty;

    [JsonProperty("wreck-description")]
    public string WreckDescription { get; set; } = string.Empty;

    [JsonProperty("claim-salvage")]
    public string ClaimSalvage { get; set; } = string.Empty;

    [JsonProperty("salvage-services")]
    public string SalvageServices { get; set; } = string.Empty;

    public OldSalvor Personal { get; set; } = new();

    [JsonProperty("wreck-materials")]
    public List<WreckMaterial> WreckMaterials { get; set; } = new();
}

public class WreckMaterial
{
    public string Description { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    [JsonProperty("value-known")]
    public string ValueKnown { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;
    public string OriginalFilename { get; set; } = string.Empty;

    [JsonProperty("address-details")]
    public AddressDetails AddressDetails { get; set; } = new();

    [JsonProperty("storage-address")]
    public string StorageAddress { get; set; } = string.Empty;
}

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

public class OldSalvor
{
    [JsonProperty("full-name")]
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [JsonProperty("telephone-number")]
    public string TelephoneNumber { get; set; } = string.Empty;

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
