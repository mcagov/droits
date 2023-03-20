using Newtonsoft.Json;

namespace Droits.Models
{

public class WreckReport
    {
        public string Reference { get; set; }

        [JsonProperty("report-date")]
        public string ReportDate { get; set; }

        [JsonProperty("wreck-find-date")]
        public string WreckFindDate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [JsonProperty("location-radius")]
        public int LocationRadius { get; set; }

        [JsonProperty("location-description")]
        public string LocationDescription { get; set; }

        [JsonProperty("vessel-name")]
        public string VesselName { get; set; }

        [JsonProperty("vessel-construction-year")]
        public string VesselConstructionYear { get; set; }

        [JsonProperty("vessel-sunk-year")]
        public string VesselSunkYear { get; set; }

        [JsonProperty("vessel-depth")]
        public string VesselDepth { get; set; }

        [JsonProperty("removed-from")]
        public string RemovedFrom { get; set; }

        [JsonProperty("wreck-description")]
        public string WreckDescription { get; set; }

        [JsonProperty("claim-salvage")]
        public string ClaimSalvage { get; set; }

        [JsonProperty("salvage-services")]
        public string SalvageServices { get; set; }
        public Salvor Personal { get; set; }

        [JsonProperty("wreck-materials")]
        public List<WreckMaterial> WreckMaterials { get; set; }
    }

    public class WreckMaterial
    {
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Value { get; set; }

        [JsonProperty("value-known")]
        public string ValueKnown { get; set; }
        public string Image { get; set; }
        public string OriginalFilename { get; set; }

        [JsonProperty("address-details")]
        public AddressDetails AddressDetails { get; set; }

        [JsonProperty("storage-address")]
        public string StorageAddress { get; set; }
    }


     public class AddressDetails
    {
        [JsonProperty("address-line-1")]
        public string Line1 { get; set; }

        [JsonProperty("address-line-2")]
        public string Line2 { get; set; }

        [JsonProperty("address-town")]
        public string Town { get; set; }

        [JsonProperty("address-county")]
        public string County { get; set; }

        [JsonProperty("address-postcode")]
        public string Postcode { get; set; }
    }

    public class Salvor
    {
        [JsonProperty("full-name")]
        public string FullName { get; set; }
        public string Email { get; set; }

        [JsonProperty("telephone-number")]
        public string TelephoneNumber { get; set; }

        [JsonProperty("address-line-1")]
        public string Line1 { get; set; }

        [JsonProperty("address-line-2")]
        public string Line2 { get; set; }

        [JsonProperty("address-town")]
        public string Town { get; set; }

        [JsonProperty("address-county")]
        public string County { get; set; }

        [JsonProperty("address-postcode")]
        public string Postcode { get; set; }
    }

}
