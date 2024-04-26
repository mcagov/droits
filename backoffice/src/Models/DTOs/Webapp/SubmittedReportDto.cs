using System.Text.Json.Serialization;

namespace Droits.Models.DTOs;

public class SubmittedReportDto
{
    [JsonPropertyName("report-date")]
    public string? ReportDate { get; set; }

    [JsonPropertyName("wreck-find-date")]
    public string? WreckFindDate { get; set; }
    
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }
    
    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
    
    [JsonPropertyName("location-radius")]
    public int? LocationRadius { get; set; }
    
    [JsonPropertyName("location-description")]
    public string? LocationDescription { get; set; }
    
    [JsonPropertyName("vessel-name")]
    public string? VesselName { get; set; }
    
    [JsonPropertyName("vessel-construction-year")]
    public string? VesselConstructionYear { get; set; }
    
    [JsonPropertyName("vessel-sunk-year")]
    public string? VesselSunkYear { get; set; }
    
    [JsonPropertyName("vessel-depth")]
    public double? VesselDepth { get; set; }
    
    [JsonPropertyName("removed-from")]
    public string? RemovedFrom { get; set; }
    
    [JsonPropertyName("wreck-description")]
    public string? WreckDescription { get; set; }
    
    [JsonPropertyName("claim-salvage")]
    public string? ClaimSalvage { get; set; }
    
    [JsonPropertyName("salvage-services")]
    public string? SalvageServices { get; set; }
    
    public SubmittedPersonalDto? Personal { get; set; }
    
    [JsonPropertyName("wreck-materials")]
    public List<SubmittedWreckMaterialDto>? WreckMaterials { get; set; }
}

public class SubmittedPersonalDto
{
    [JsonPropertyName("full-name")]
    public string? FullName { get; set; }
    
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    [JsonPropertyName("telephone-number")]
    public string? TelephoneNumber { get; set; }
    
    [JsonPropertyName("address-line-1")]
    public string? AddressLine1 { get; set; }
    
    [JsonPropertyName("address-line-2")]
    public string? AddressLine2 { get; set; }
    
    [JsonPropertyName("address-town")]
    public string? AddressTown { get; set; }
    
    [JsonPropertyName("address-county")]
    public string? AddressCounty { get; set; }
    
    [JsonPropertyName("address-postcode")]
    public string? AddressPostcode { get; set; }
}

public class SubmittedWreckMaterialDto
{
    [JsonPropertyName("droit-id")]
    public Guid? DroitId { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("quantity")]
    public string? Quantity { get; set; }
    
    [JsonPropertyName("value")]
    public double? Value { get; set; }
    
    [JsonPropertyName("value-known")]
    public string? ValueKnown { get; set; }
    
    
    [JsonPropertyName("image")]
    public SubmittedImageDto? Image { get; set; }
    
    [JsonPropertyName("address-details")]
    public SubmittedAddressDetailsDto? AddressDetails { get; set; }
    
    [JsonPropertyName("storage-address")]
    public string? StorageAddress { get; set; }
}

public class SubmittedImageDto
{
    [JsonPropertyName("filename")]
    public string? Filename { get; set; }
    
    [JsonPropertyName("data")]
    public string? Data { get; set; }
}
public class SubmittedAddressDetailsDto
{
    [JsonPropertyName("address-line-1")]
    public string? AddressLine1 { get; set; }
    
    [JsonPropertyName("address-line-2")]
    public string? AddressLine2 { get; set; }
    
    [JsonPropertyName("address-town")]
    public string? AddressTown { get; set; }
    
    [JsonPropertyName("address-county")]
    public string? AddressCounty { get; set; }
    
    [JsonPropertyName("address-postcode")]
    public string? AddressPostcode { get; set; }
}