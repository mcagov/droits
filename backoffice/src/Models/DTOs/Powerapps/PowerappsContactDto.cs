using System.Text.Json.Serialization;
using Droits.Helpers;

namespace Droits.Models.DTOs.Powerapps
{
    public class PowerappsContactDto
    {
        [JsonPropertyName("contactid")]
        public string? ContactId { get; set; }

        [JsonPropertyName("fullname")]
        public string? FullName { get; set; }

        [JsonPropertyName("emailaddress1")]
        public string? EmailAddress { get; set; }

        [JsonPropertyName("telephone1")]
        public string? Telephone1 { get; set; }

        [JsonPropertyName("telephone2")]
        public string? Telephone2 { get; set; }

        [JsonPropertyName("telephone3")]
        public string? Telephone3 { get; set; }

        [JsonPropertyName("mobilephone")]
        public string? MobilePhone { get; set; }

        [JsonPropertyName("address1_line1")]
        public string? AddressLine1 { get; set; }

        [JsonPropertyName("address1_line2")]
        public string? AddressLine2 { get; set; }

        [JsonPropertyName("address1_line3")]
        public string? AddressLine3 { get; set; }

        [JsonPropertyName("address1_city")]
        public string? AddressCity { get; set; }

        [JsonPropertyName("address1_county")]
        public string? AddressCounty { get; set; }

        [JsonPropertyName("address1_country")]
        public string? AddressCountry { get; set; }

        [JsonPropertyName("address1_postalcode")]
        public string? AddressPostalCode { get; set; }

        [JsonPropertyName("address1_composite")]
        public string? AddressComposite { get; set; }
        
        public string? GetContactDetails()
        {
            return StringHelper.JoinWithSeparator("\n",  EmailAddress, AddressLine1, AddressLine2, AddressLine3,
                AddressCity, AddressCounty, AddressCountry, AddressPostalCode,
                Telephone1, Telephone2, Telephone3, MobilePhone);
        }
    }
}
