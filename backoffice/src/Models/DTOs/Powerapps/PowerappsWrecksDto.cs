using System.Text.Json.Serialization;
using Droits.Models.Enums;


//https://reportwreckmaterial.crm11.dynamics.com/api/data/v9.2/crf99_mcawreckses?$select=crf99_mcawrecksid,crf99_name,crf99_wrecktype,crf99_longitude,crf99_latitude,crf99_iswarwreck,crf99_isaircraft,createdon,crf99_dateofloss,crf99_protectedsite,_crf99_protectionlegislation_value&$expand=crf99_WreckOwner($select=fullname,emailaddress1,address1_line1,address1_city,address1_postalcode,address1_composite,mobilephone,createdon)

namespace Droits.Models.DTOs.Powerapps
{
    public class PowerappsWrecksDto
    {
        [JsonPropertyName("value")]
        public List<PowerappsWreckDto>? Value { get; set; }
    }

    public class PowerappsWreckDto
    {
        [JsonPropertyName("crf99_mcawrecksid")]
        public string? Mcawrecksid { get; set; }

        [JsonPropertyName("createdon")]
        public DateTime? CreatedOn { get; set; }

        [JsonPropertyName("crf99_name")]
        public string? Name { get; set; }

        [JsonPropertyName("crf99_wrecktype")]
        public int? WreckType { get; set; } // 614880000 is Historic, 614880001 is Modern

        [JsonPropertyName("crf99_isaircraft")]
        public bool? IsAircraft { get; set; }

        [JsonPropertyName("crf99_protectedsite")]
        public bool? ProtectedSite { get; set; }

        [JsonPropertyName("crf99_iswarwreck")]
        public bool? IsWarWreck { get; set; }

        [JsonPropertyName("crf99_dateofloss")]
        public DateTime? DateOfLoss { get; set; }

        [JsonPropertyName("_crf99_protectionlegislation_value")]
        public string? ProtectionLegislationValue { get; set; }

        [JsonPropertyName("crf99_longitude")]
        public double? Longitude { get; set; }

        [JsonPropertyName("crf99_latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("crf99_WreckOwner")]
        public PowerappsWreckOwnerDto? WreckOwner { get; set; }

        public WreckType? GetWreckType() => WreckType switch
        {
            614880000 => Enums.WreckType.Historic,
            614880001 => Enums.WreckType.Modern,
            _ => null
        };
        
        public string? GetProtectedLegislation() => ProtectionLegislationValue switch
        {
            "6b097d27-c525-ec11-b6e6-000d3ad65574" => "Historic Monuments and Archaeological Objects (Northern Ireland) Order 1995",
            "cec2eab2-ac85-ec11-8d21-00224842d40e" => "Protection of Wrecks Act 1973",
            _ => null
        };
    }

    public class PowerappsWreckOwnerDto // WIP - address & phone, unsure.. 
    {
        [JsonPropertyName("fullname")]
        public string? FullName { get; set; }

        [JsonPropertyName("emailaddress1")]
        public string? EmailAddress { get; set; }

        [JsonPropertyName("address1_composite")]
        public string? AddressComposite { get; set; }

        [JsonPropertyName("mobilephone")] // This is the only telephone number currently used.
        public string? MobilePhone { get; set; }
    }
}
