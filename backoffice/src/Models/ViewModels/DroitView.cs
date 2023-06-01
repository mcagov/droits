using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Droits.Models
{
    public class DroitView
    {

        public DroitView() { }
        public DroitView(Droit droit)
        {
            this.Id = droit.Id;
            this.Status = droit.Status;
            this.ReportedDate = droit.ReportedDate;
            this.Created = droit.Created;
            this.Modified = droit.Modified;
            this.Reference = droit.Reference;

            this.IsHazardousFind = droit.IsHazardousFind;

            //Wreck
            this.WreckConstructionDetails = droit.WreckConstructionDetails;
            this.WreckVesselName = droit.WreckVesselName;
            this.WreckVesselYearConstructed = droit.WreckVesselYearConstructed;
            this.WreckVesselYearSunk = droit.WreckVesselYearSunk;

            // Location
            this.Latitude = droit.Latitude;
            this.Longitude = droit.Longitude;
            this.InUkWaters = droit.InUkWaters;
            this.LocationRadius = droit.LocationRadius;
            this.Depth = droit.Depth;
            this.LocationDescription = droit.LocationDescription;

            // Salvage

            this.SalvageAwardClaimed = droit.SalvageAwardClaimed;
            this.ServicesDescription = droit.ServicesDescription;
            this.ServicesDuration = droit.ServicesDuration;
            this.ServicesEstimatedCost = droit.ServicesEstimatedCost;
            this.MMOLicenceRequired = droit.MMOLicenceRequired;
            this.MMOLicenceProvided = droit.MMOLicenceProvided;
            this.SalvageClaimAwarded = droit.SalvageClaimAwarded;

            // Legacy fields

            this.District = droit.District;
            this.LegacyFileReference = droit.LegacyFileReference;
            this.GoodsDischargedBy = droit.GoodsDischargedBy;
            this.DateDelivered = droit.DateDelivered;
            this.Agent = droit.Agent;
            this.RecoveredFrom = droit.RecoveredFrom;
            this.ImportedFromLegacy = droit.ImportedFromLegacy;
        }
        // Base fields...

        public Guid Id{get;set;}

        public string? Reference {get;set;} // This is the current reference.

        public DroitStatus Status {get;set;} = DroitStatus.Received;

        [DisplayName("Reported Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReportedDate{get;set;}

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Created{get;set;}

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Modified{get;set;}


        // Wreck

        public Guid? WreckId{get;set;}
        [DisplayName("Wreck Construction Details")]
        public string? WreckConstructionDetails {get;set;} = string.Empty;

        [DisplayName("Is Hazardous Find")]
        public bool IsHazardousFind {get;set;} = false;


        // Wreck Vessel

        [DisplayName("Wreck Vessel Name")]
        public string? WreckVesselName {get;set;} = string.Empty;

        [DisplayName("Wreck Vessel Year Constructed")]
        public int? WreckVesselYearConstructed {get;set;}

        [DisplayName("Wreck Vessel Year Sunk")]
        public int? WreckVesselYearSunk{get;set;}


        // Location
        public string? Latitude {get;set;}
        public string? Longitude{get;set;}

        [DisplayName("In UK Waters?")]

        public bool InUkWaters {get;set;} = false;
        [DisplayName("Location Radius (Units?)")]
        public int? LocationRadius {get;set;} //Units? Unknown.
        [DisplayName("Depth (Units?)")]
        public int? Depth{get;set;} //Units? Unknown.
        [DisplayName("Location Description")]
        public string? LocationDescription {get;set;} = string.Empty;


        // Salvage

        [DisplayName("Salvage Award Claimed")]
        public bool SalvageAwardClaimed {get;set;} = false;
        [DisplayName("Services Description")]

        public string? ServicesDescription {get;set;}

        [DisplayName("Services Duration")]
        public int? ServicesDuration {get;set;} //Units? Unknown.

        [DisplayName("Services Estimated Cost")]

        public float? ServicesEstimatedCost {get;set;}

        [DisplayName("MMO Licence Required")]
        public bool MMOLicenceRequired {get;set;} = false;
        [DisplayName("MMO Licence Provided")]
        public bool MMOLicenceProvided {get;set;} = false;

        [DisplayName("Salvage Claim Awarded")]
        public float SalvageClaimAwarded {get;set;} = 0f;

        // Legacy fields
        public string? District {get;set;}

        [DisplayName("Legacy File Reference")]
        public string? LegacyFileReference {get;set;} //Physical file location/ref

        [DisplayName("Goods Discharged By")]
        public string? GoodsDischargedBy{get;set;} //Initals of RoW member.

        [DisplayName("Date Delivered")]
        public string? DateDelivered {get;set;} //Unsure of date format.
        public string? Agent {get;set;}

        [DisplayName("Recovered From")]
        public string? RecoveredFrom {get;set;}

        [DisplayName("Imported From Legacy")]
        public bool ImportedFromLegacy {get;set;} = false;

    }
}
