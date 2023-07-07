using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Models.ViewModels;

public class DroitView
{
    public DroitView()
    {
    }

    public DroitView(Droit droit)
    {
        Id = droit.Id;
        Status = droit.Status;
        ReportedDate = droit.ReportedDate;
        Created = droit.Created;
        LastModified = droit.LastModified;
        Reference = droit.Reference;

        IsHazardousFind = droit.IsHazardousFind;

        //Wreck
        if(droit.Wreck != null){
            Wreck = new WreckView(droit.Wreck);
        }

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

    public string? Reference { get; } // This is the current reference.

    public DroitStatus Status { get; set; } = DroitStatus.Received;

    [DisplayName("Reported Date")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime ReportedDate { get; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime Created { get; }

    [DisplayName("Last Modified")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime LastModified { get; }


    // Wreck

    public WreckView? Wreck {get;}
    public Guid? WreckId { get; }

    [DisplayName("Wreck Construction Details")]
    public string? WreckConstructionDetails { get; } = string.Empty;

    [DisplayName("Is Hazardous Find")]
    public bool IsHazardousFind { get; }


    // Wreck Vessel

    [DisplayName("Wreck Vessel Name")]
    public string? WreckVesselName { get; } = string.Empty;

    [DisplayName("Wreck Vessel Year Constructed")]
    public int? WreckVesselYearConstructed { get; }

    [DisplayName("Wreck Vessel Year Sunk")]
    public int? WreckVesselYearSunk { get; }


    // Location
    public string? Latitude { get; }
    public string? Longitude { get; }

    [DisplayName("In UK Waters?")]
    public bool InUkWaters { get; }

    [DisplayName("Location Radius (Units?)")]
    public int? LocationRadius { get; } //Units? Unknown.

    [DisplayName("Depth (Units?)")]
    public int? Depth { get; } //Units? Unknown.

    [DisplayName("Location Description")]
    public string? LocationDescription { get; } = string.Empty;


    // Salvage

    [DisplayName("Salvage Award Claimed")]
    public bool SalvageAwardClaimed { get; }

    [DisplayName("Services Description")]
    public string? ServicesDescription { get; }

    [DisplayName("Services Duration")]
    public int? ServicesDuration { get; } //Units? Unknown.

    [DisplayName("Services Estimated Cost")]

    public float? ServicesEstimatedCost { get; }

    [DisplayName("MMO Licence Required")]
    public bool MMOLicenceRequired { get; }

    [DisplayName("MMO Licence Provided")]
    public bool MMOLicenceProvided { get; }

    [DisplayName("Salvage Claim Awarded")]
    public float SalvageClaimAwarded { get; }

    // Legacy fields
    public string? District { get; }

    [DisplayName("Legacy File Reference")]
    public string? LegacyFileReference { get; } //Physical file location/ref

    [DisplayName("Goods Discharged By")]
    public string? GoodsDischargedBy { get; } //Initals of RoW member.

    [DisplayName("Date Delivered")]
    public string? DateDelivered { get; } //Unsure of date format.

    public string? Agent { get; }

    [DisplayName("Recovered From")]
    public string? RecoveredFrom { get; }

    [DisplayName("Imported From Legacy")]
    public bool ImportedFromLegacy { get; }
}
