namespace Droits.Models
{
    public class Droit
    {
        // Base fields...

        public Guid Id{get;set;}

        public string? Reference {get;set;} // This is the current reference.
        public DroitStatus Status {get;set;} = DroitStatus.Received;
        public DateTime ReportedDate{get;set;} = DateTime.UtcNow;
        public DateTime Created{get;set;} = DateTime.UtcNow;
        public DateTime Modified{get;set;} = DateTime.UtcNow;


        // public List<DroitItem> WreckMaterials {get;set;} = new List<DroitItem>();

        // Wreck

        public Guid? WreckId{get;set;}

        public bool IsHazardousFind {get;set;} = false;

        // Wreck Vessel

        public string? WreckVesselName {get;set;} = string.Empty;
        public string? WreckConstructionDetails {get;set;} = string.Empty;
        public int? WreckVesselYearConstructed {get;set;}
        public int? WreckVesselYearSunk{get;set;}


        // Location
        public string? Latitude {get;set;}
        public string? Longitude{get;set;}
        public bool InUkWaters {get;set;} = false;
        public int? LocationRadius {get;set;} //Units? Unknown.
        public int? Depth{get;set;} //Units? Unknown. //metres from frontend
        public string? LocationDescription {get;set;} = string.Empty;


        // Salvage

        public bool SalvageAwardClaimed {get;set;} = false;
        public string? ServicesDescription {get;set;}
        public int? ServicesDuration {get;set;} //Units? Unknown.
        public float? ServicesEstimatedCost {get;set;}
        public bool MMOLicenceRequired {get;set;} = false;
        public bool MMOLicenceProvided {get;set;} = false;
        public float SalvageClaimAwarded {get;set;} = 0f;

        // Legacy fields
        public string? District {get;set;}
        public string? LegacyFileReference {get;set;} //Physical file location/ref
        public string? GoodsDischargedBy{get;set;} //Initals of RoW member.
        public string? DateDelivered {get;set;} //Unsure of date format.
        public string? Agent {get;set;}
        public string? RecoveredFrom {get;set;}
        public bool ImportedFromLegacy {get;set;} = false;


    }

}
