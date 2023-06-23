using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Droits.Models;

public class DroitForm : FormModel
{
    public DroitForm()
    {
    }

    public DroitForm(Droit droit)
    {
        Id = droit.Id;
        Status = droit.Status;
        ReportedDate = droit.ReportedDate;
        Created = droit.Created;
        Modified = droit.LastModified;
        Reference = droit.Reference;
        IsHazardousFind = droit.IsHazardousFind;

        //Wreck
        WreckConstructionDetails = droit.WreckConstructionDetails;
        WreckVesselName = droit.WreckVesselName;
        WreckVesselYearConstructed = droit.WreckVesselYearConstructed;
        WreckVesselYearSunk = droit.WreckVesselYearSunk;

        // Location
        Latitude = droit.Latitude;
        Longitude = droit.Longitude;
        InUkWaters = droit.InUkWaters;
        LocationRadius = droit.LocationRadius;
        Depth = droit.Depth;
        LocationDescription = droit.LocationDescription;

        // Salvage

        SalvageAwardClaimed = droit.SalvageAwardClaimed;
        ServicesDescription = droit.ServicesDescription;
        ServicesDuration = droit.ServicesDuration;
        ServicesEstimatedCost = droit.ServicesEstimatedCost;
        MMOLicenceRequired = droit.MMOLicenceRequired;
        MMOLicenceProvided = droit.MMOLicenceProvided;
        SalvageClaimAwarded = droit.SalvageClaimAwarded;

        // Legacy fields

        District = droit.District;
        LegacyFileReference = droit.LegacyFileReference;
        GoodsDischargedBy = droit.GoodsDischargedBy;
        DateDelivered = droit.DateDelivered;
        Agent = droit.Agent;
        RecoveredFrom = droit.RecoveredFrom;
        ImportedFromLegacy = droit.ImportedFromLegacy;
    }


    // Base fields...

    public Guid Id { get; set; }

    [Required]
    public string Reference { get; set; } = string.Empty;

    public DroitStatus Status { get; set; } = DroitStatus.Received;

    [DisplayName("Reported Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ReportedDate { get; set; } = DateTime.Now;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Created { get; set; } = DateTime.Now;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Modified { get; set; } = DateTime.Now;


    // Wreck

    public Guid? WreckId { get; set; }

    [DisplayName("Wreck Construction Details")]
    public string? WreckConstructionDetails { get; set; } = string.Empty;

    [DisplayName("Is Hazardous Find")]
    public bool IsHazardousFind { get; set; }


    // Wreck Vessel

    [DisplayName("Wreck Vessel Name")]
    public string? WreckVesselName { get; set; } = string.Empty;

    [DisplayName("Wreck Vessel Year Constructed")]
    public int? WreckVesselYearConstructed { get; set; }

    [DisplayName("Wreck Vessel Year Sunk")]
    public int? WreckVesselYearSunk { get; set; }


    // Location
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }

    [DisplayName("In UK Waters?")]
    public bool InUkWaters { get; set; }

    [DisplayName("Location Radius (Units?)")]
    public int? LocationRadius { get; set; } //Units? Unknown.

    [DisplayName("Depth (Units?)")]
    public int? Depth { get; set; } //Units? Unknown.

    [DisplayName("Location Description")]
    public string? LocationDescription { get; set; } = string.Empty;


    // Salvage

    [DisplayName("Salvage Award Claimed")]
    public bool SalvageAwardClaimed { get; set; }

    [DisplayName("Services Description")]
    public string? ServicesDescription { get; set; }

    [DisplayName("Services Duration")]
    public int? ServicesDuration { get; set; } //Units? Unknown.

    [DisplayName("Services Estimated Cost")]

    public float? ServicesEstimatedCost { get; set; }

    [DisplayName("MMO Licence Required")]
    public bool MMOLicenceRequired { get; set; }

    [DisplayName("MMO Licence Provided")]
    public bool MMOLicenceProvided { get; set; }

    [DisplayName("Salvage Claim Awarded")]
    public float SalvageClaimAwarded { get; set; }

    // Legacy fields
    public string? District { get; set; }

    [DisplayName("Legacy File Reference")]
    public string? LegacyFileReference { get; set; } //Physical file location/ref

    [DisplayName("Goods Discharged By")]
    public string? GoodsDischargedBy { get; set; } //Initals of RoW member.

    [DisplayName("Date Delivered")]
    public string? DateDelivered { get; set; } //Unsure of date format.

    public string? Agent { get; set; }

    [DisplayName("Recovered From")]
    public string? RecoveredFrom { get; set; }

    [DisplayName("Imported From Legacy")]
    public bool ImportedFromLegacy { get; set; }

    public Droit ApplyChanges(Droit droit)
    {
        droit.Id = Id;
        droit.Status = Status;
        droit.ReportedDate = ReportedDate;
        droit.Created = Created;
        droit.LastModified = Modified;
        droit.Reference = Reference;

        droit.WreckConstructionDetails = WreckConstructionDetails;
        droit.IsHazardousFind = IsHazardousFind;

        //Wreck
        droit.WreckVesselName = WreckVesselName;
        droit.WreckVesselYearConstructed = WreckVesselYearConstructed;
        droit.WreckVesselYearSunk = WreckVesselYearSunk;

        // Location
        droit.Latitude = Latitude;
        droit.Longitude = Longitude;
        droit.InUkWaters = InUkWaters;
        droit.LocationRadius = LocationRadius;
        droit.Depth = Depth;
        droit.LocationDescription = LocationDescription;

        // Salvage

        droit.SalvageAwardClaimed = SalvageAwardClaimed;
        droit.ServicesDescription = ServicesDescription;
        droit.ServicesDuration = ServicesDuration;
        droit.ServicesEstimatedCost = ServicesEstimatedCost;
        droit.MMOLicenceRequired = MMOLicenceRequired;
        droit.MMOLicenceProvided = MMOLicenceProvided;
        droit.SalvageClaimAwarded = SalvageClaimAwarded;

        // Legacy fields

        droit.District = District;
        droit.LegacyFileReference = LegacyFileReference;
        droit.GoodsDischargedBy = GoodsDischargedBy;
        droit.DateDelivered = DateDelivered;
        droit.Agent = Agent;
        droit.RecoveredFrom = RecoveredFrom;
        droit.ImportedFromLegacy = ImportedFromLegacy;

        return droit;
    }
}
