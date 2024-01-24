using System.Text.Json.Serialization;
using Droits.Models.Enums;

// https://reportwreckmaterial.crm11.dynamics.com/api/data/v9.2/crf99_mcawreckmaterials?$select=crf99_mcawreckmaterialid,_crf99_wreckreport_value,createdon,crf99_name,crf99_description,crf99_purchasedbymuseum,crf99_receivervaluation,crf99_valueconfirmed,crf99_imageurl,crf99_quantity,crf99_value,crf99_outcome,crf99_outcomelegacy,crf99_outcomeremarks,crf99_purchaserlegacy,crf99_wheresecuredlegacy,crf99_wreckmaterialownerlegacy&$expand=crf99_StorageAddress($select=crf99_addressline1,crf99_addressline2,crf99_city,crf99_county,crf99_postcode),crf99_Purchaser($select=fullname,emailaddress1,telephone1,telephone2,telephone3,mobilephone,address1_line1,address1_line2,address1_line3,address1_city,address1_county,address1_country,address1_postalcode,address1_composite),crf99_WreckMaterialOwner($select=fullname,emailaddress1,telephone1,telephone2,telephone3,mobilephone,address1_line1,address1_line2,address1_line3,address1_city,address1_county,address1_country,address1_postalcode,address1_composite)

namespace Droits.Models.DTOs.Powerapps
{
    public class PowerappsWreckMaterialsDto
    {
        [JsonPropertyName("value")]
        public List<PowerappsWreckMaterialDto>? Value { get; set; }
    }

    public class PowerappsWreckMaterialDto
    {

        [JsonPropertyName("crf99_mcawreckmaterialid")]
        public string? PowerappsWreckMaterialId { get; set; } //Droit ID, to attach to droit

        [JsonPropertyName("_crf99_wreckreport_value")]
        public string? PowerappsDroitId { get; set; } //Droit ID, to attach to droit


        [JsonPropertyName("createdon")]
        public DateTime? CreatedOn { get; set; }

        [JsonPropertyName("crf99_name")]
        public string? Name { get; set; }

        [JsonPropertyName("crf99_description")]
        public string? Description { get; set; }

        [JsonPropertyName("crf99_purchasedbymuseum")]
        public bool? PurchasedByMuseum { get; set; }

        [JsonPropertyName("crf99_receivervaluation")]
        public double? ReceiverValuation { get; set; }

        [JsonPropertyName("crf99_valueconfirmed")]
        public bool? ValueConfirmed { get; set; }

        [JsonPropertyName("crf99_quantity")]
        public int? Quantity { get; set; }

        [JsonPropertyName("crf99_value")]
        public decimal? Value { get; set; }

        [JsonPropertyName("crf99_outcome")]
        public int? Outcome { get; set; } // Lookup

        [JsonPropertyName("crf99_outcomelegacy")]
        public string? OutcomeLegacy { get; set; }

        [JsonPropertyName("crf99_outcomeremarks")]
        public string? OutcomeRemarks { get; set; }

        [JsonPropertyName("crf99_purchaserlegacy")]
        public string? PurchaserLegacy { get; set; }


        [JsonPropertyName("crf99_wreckmaterialownerlegacy")]
        public string? WreckMaterialOwnerLegacy { get; set; }

        [JsonPropertyName("crf99_WreckMaterialOwner")]
        public PowerappsContactDto? WreckMaterialOwner { get; set; }

        [JsonPropertyName("crf99_Purchaser")]
        public PowerappsContactDto? Purchaser { get; set; }

        [JsonPropertyName("crf99_StorageAddress")]
        public PowerappsStorageAddressDto? StorageAddress { get; set; }
        [JsonPropertyName("crf99_wheresecuredlegacy")]
        public string? WhereSecuredLegacy { get; set; }


        [JsonPropertyName("crf99_imageurl")]
        public string? ImageUrl { get; set; }
        
        public WreckMaterialOutcome? GetOutcome()
        {

            if ( PurchasedByMuseum == true )
            {
                return WreckMaterialOutcome.SoldToMuseum;
            }

            return Outcome switch
            {
                614880000 => WreckMaterialOutcome.LieuOfSalvage,
                614880001 => WreckMaterialOutcome.ReturnedToOwner,
                614880002 => WreckMaterialOutcome.DonatedToMuseum,
                614880003 => WreckMaterialOutcome.SoldToMuseum,
                614880004 => WreckMaterialOutcome.Other,
                _ => null
            };
        }
    }

    public class PowerappsStorageAddressDto
    {
        [JsonPropertyName("crf99_addressline1")]
        public string? AddressLine1 { get; set; }

        [JsonPropertyName("crf99_addressline2")]
        public string? AddressLine2 { get; set; }

        [JsonPropertyName("crf99_city")]
        public string? City { get; set; }
        
        [JsonPropertyName("crf99_county")]
        public string? County { get; set; }
        
        [JsonPropertyName("crf99_postcode")]
        public string? Postcode { get; set; }


    }
}
