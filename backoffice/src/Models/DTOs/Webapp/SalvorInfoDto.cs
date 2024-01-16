namespace Droits.Models.DTOs.Webapp;

using System.Text.Json.Serialization;

public class SalvorInfoDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    [JsonPropertyName("telephoneNumber")]
    public string? TelephoneNumber { get; set; }
    
    [JsonPropertyName("address")]
    public SalvorInfoAddressDto? Address { get; set; }
    
    [JsonPropertyName("reports")]
    public SalvorInfoReportDto[]? Reports { get; set; }
}

public class SalvorInfoAddressDto
{
    [JsonPropertyName("line1")]
    public string? Line1 { get; set; }

    [JsonPropertyName("line2")]
    public string? Line2 { get; set; }

    [JsonPropertyName("city")]
    public string? Town { get; set; }

    [JsonPropertyName("county")]
    public string? County { get; set; }

    [JsonPropertyName("postcode")]
    public string? Postcode { get; set; }
}

public class SalvorInfoReportDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("reference")]
    public string? Reference { get; set; }
    
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    
    [JsonPropertyName("recovered_from")]
    public string? RecoveredFrom { get; set; }
    
    [JsonPropertyName("coordinates")]
    public string? Coordinates { get; set; }
    
    [JsonPropertyName("date_found")]
    public string? DateFound { get; set; }
    
    [JsonPropertyName("date_reported")]
    public string? DateReported { get; set; }
    
    [JsonPropertyName("last_updated")]
    public string? LastUpdated { get; set; }
    
    [JsonPropertyName("wreck_materials")]
    public SalvorInfoWreckMaterialDto[]? WreckMaterials { get; set; }
}

public class SalvorInfoWreckMaterialDto
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("outcome")]
    public string? Outcome { get; set; }

    [JsonPropertyName("storage_address")]
    public SalvorInfoAddressDto? StorageAddress { get; set; }
}