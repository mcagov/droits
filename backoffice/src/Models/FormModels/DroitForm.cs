#region

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

#endregion

namespace Droits.Models.FormModels;

public class DroitForm : BaseEntityForm
{
    public DroitForm()
    {
    }


    public DroitForm(Droit droit) : base(droit)
    {
        AssignedToUserId = droit.AssignedToUserId;
        WreckId = droit.WreckId;
        SalvorId = droit.SalvorId;

        Status = droit.Status;
        ReportedDate = droit.ReportedDate;
        DateFound = droit.DateFound;

        IsIsolatedFind = droit.WreckId == default;
        
        Reference = droit.Reference;
        IsHazardousFind = droit.IsHazardousFind;
        IsDredge = droit.IsDredge;
        
        OriginalSubmission = droit.OriginalSubmission;
        

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
        MmoLicenceRequired = droit.MmoLicenceRequired;
        MmoLicenceProvided = droit.MmoLicenceProvided;
        SalvageClaimAwarded = droit.SalvageClaimAwarded;

        // Legacy fields

        District = droit.District;
        LegacyFileReference = droit.LegacyFileReference;
        GoodsDischargedBy = droit.GoodsDischargedBy;
        DateDelivered = droit.DateDelivered;
        Agent = droit.Agent;
        RecoveredFrom = droit.RecoveredFrom;
        ImportedFromLegacy = droit.ImportedFromLegacy;


        if ( droit.WreckMaterials.Any() )
        {
            WreckMaterialForms =
                droit.WreckMaterials.Select(wm => new WreckMaterialForm(wm)).ToList();
        }
    }


    // Base fields...
    public WreckForm WreckForm { get; set; } = new();

    [DisplayName("Is Isolated Find")]
    public bool IsIsolatedFind { get; set; } = true;

    public List<WreckMaterialForm> WreckMaterialForms { get; set; } = new();
    public SalvorForm SalvorForm { get; set; } = new();

    [Required]
    public string Reference { get; set; } = string.Empty;

    public DroitStatus Status { get; set; } = DroitStatus.Received;
    
    [DisplayName("Assigned To")]
    public Guid? AssignedToUserId { get; set; }
    
    [DisplayName("Reported Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ReportedDate { get; set; } = DateTime.UtcNow;

    [DisplayName("Date Found")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DateFound { get; set; } = DateTime.UtcNow;

    public string? OriginalSubmission { get; set; } = string.Empty;


    // Wreck

    public Guid? WreckId { get; set; }

    [DisplayName("Is Hazardous Find")]
    public bool IsHazardousFind { get; set; }

    [DisplayName("Is Dredge")]
    public bool IsDredge { get; set; }

    // Salvor

    public Guid? SalvorId { get; set; }


    // Location
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [DisplayName("In UK Waters")]
    public bool InUkWaters { get; set; }

    [DisplayName("Location Radius")]
    public int? LocationRadius { get; set; }

    [DisplayName("Depth (Metres)")]
    public int? Depth { get; set; }
    
    [DisplayName("Recovered From")]
    public RecoveredFrom? RecoveredFrom { get; set; }

    [DisplayName("Location Description")]
    public string? LocationDescription { get; set; } = string.Empty;


    // Salvage

    [DisplayName("Salvage Award Claimed")]
    public bool SalvageAwardClaimed { get; set; }

    [DisplayName("Services Description")]
    public string? ServicesDescription { get; set; }

    [DisplayName("Services Duration")]
    public string? ServicesDuration { get; set; } //Units? Unknown.

    [DisplayName("Services Estimated Cost")]

    public double? ServicesEstimatedCost { get; set; }

    [DisplayName("MMO Licence Required")]
    public bool MmoLicenceRequired { get; set; }

    [DisplayName("MMO Licence Provided")]
    public bool MmoLicenceProvided { get; set; }

    [DisplayName("Salvage Claim Awarded")]
    public double SalvageClaimAwarded { get; set; }

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
    public string? RecoveredFromLegacy { get; set; }

    [DisplayName("Imported From Legacy")]
    public bool ImportedFromLegacy { get; set; }

    public List<SelectListItem> AllUsers { get; set; } = new();
    public List<SelectListItem> AllWrecks { get; set; } = new();
    public List<SelectListItem> AllSalvors { get; set; } = new();


    public Droit ApplyChanges(Droit droit)
    {
        
        base.ApplyChanges(droit);
        droit.AssignedToUserId = AssignedToUserId;
        droit.WreckId = IsIsolatedFind ? default : WreckId;
        droit.SalvorId = SalvorId;
        droit.Status = Status;
        droit.RecoveredFrom = RecoveredFrom;
        droit.ReportedDate = ReportedDate;
        droit.DateFound = DateFound;
        droit.Reference = Reference;
        droit.IsHazardousFind = IsHazardousFind;
        droit.IsDredge = IsDredge;

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
        droit.MmoLicenceRequired = MmoLicenceRequired;
        droit.MmoLicenceProvided = MmoLicenceProvided;
        droit.SalvageClaimAwarded = SalvageClaimAwarded;

        // Legacy fields

        droit.District = District;
        droit.LegacyFileReference = LegacyFileReference;
        droit.GoodsDischargedBy = GoodsDischargedBy;
        droit.DateDelivered = DateDelivered;
        droit.Agent = Agent;
        droit.RecoveredFromLegacy = RecoveredFromLegacy;
        droit.ImportedFromLegacy = ImportedFromLegacy;

        return droit;
    }
}