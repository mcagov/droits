#region

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.ViewModels.ListViews;

#endregion

namespace Droits.Models.ViewModels;

public class DroitView : BaseEntityView
{
    public DroitView()
    {
    }


    public DroitView(Droit droit) : base(droit)
    {
        Id = droit.Id;
        Status = droit.Status;
        TriageNumber = droit.TriageNumber;
        ReportedDate = droit.ReportedDate;
        DateFound = droit.DateFound;
        ClosedDate = droit.ClosedDate;
        StatutoryDeadline = droit.ReportedDate.AddYears(1);
        DaysTakenToReport = droit.DaysTakenToReport;

        AssignedUser = droit.AssignedToUser?.Name ?? "Unassigned";
        
        
        Reference = droit.Reference;

        IsHazardousFind = droit.IsHazardousFind;
        IsDredge = droit.IsDredge;

        OriginalSubmission = droit.OriginalSubmission;
        ReportedWreckInfo = new ReportedWreckInfoView(droit);

        
        //Wreck
        if ( droit.Wreck != null )
        {
            Wreck = new WreckView(droit.Wreck)
            {
                Notes =
                {
                    Editable = false
                }
            };
        }

        //Salvor
        if ( droit.Salvor != null )
        {
            Salvor = new SalvorView(droit.Salvor)
            {
                Notes =
                {
                    Editable = false
                }
            };
        }

        if ( droit.WreckMaterials.Any() )
        {
            WreckMaterials = droit.WreckMaterials.Select(wm => new WreckMaterialView(wm)).OrderBy(w => w.Created).ToList();
        }

        if ( droit.Letters.Any() )
        {
            Letters = new LetterListView(droit.Letters.Select(l => new LetterView(l)).OrderByDescending(l => l.Created)
                .ToList());
        }

        if ( droit.Notes.Any() )
        {
            Notes = new NoteListView(droit.Notes.Select(n => new NoteView(n)).OrderByDescending(l => l.Created).ToList());
            Notes.ObjectReference = droit.Reference;
        }

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
        LegacyRemarks = droit.LegacyRemarks;
    }


    // Base fields...


    public Guid Id { get; }

    public string? Reference { get; } // This is the current reference.

    public DroitStatus Status { get; } = DroitStatus.Received;
    
    [DisplayName("Triage Number")]
    public int? TriageNumber { get; set; }
    
    [DisplayName("Reported Date")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime ReportedDate { get; }
    
    [DisplayName("Statutory Deadline")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime StatutoryDeadline { get; }
    
    [DisplayName("Date Found")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DateFound { get; }
    
    [DisplayName("Closed Date")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? ClosedDate { get; }
    
    [DisplayName("Days Taken To Report")]
    public int DaysTakenToReport { get; }
    
    public ReportedWreckInfoView ReportedWreckInfo = new ();

    [DisplayName("Assigned To")]
    public string AssignedUser { get; } = "Unassigned";
    
    public string? OriginalSubmission { get; set; } = string.Empty;

    // Wreck Material

    public List<WreckMaterialView> WreckMaterials { get; } = new();

    // Letters

    public LetterListView Letters { get; } = new();
    
    //Notes
    public NoteListView Notes { get; } = new();

    // Wreck

    public WreckView? Wreck { get; }
    public Guid? WreckId { get; }

    [DisplayName("Is Hazardous Find")]
    public bool IsHazardousFind { get; }

    [DisplayName("Is Dredge")]
    public bool IsDredge { get; set; }

    // Salvor

    public SalvorView? Salvor { get; }
    public Guid? SalvorId { get; }


    // Location
    public double? Latitude { get; }
    public double? Longitude { get; }

    [DisplayName("In UK Waters")]
    public bool InUkWaters { get; }

    [DisplayName("Location Radius")]
    public int? LocationRadius { get; }

    [DisplayName("Depth (Metres)")]
    public int? Depth { get; }
    
    [DisplayName("Recovered From")]
    public RecoveredFrom? RecoveredFrom { get; }

    [DisplayName("Location Description")]
    public string? LocationDescription { get; } = string.Empty;


    // Salvage

    [DisplayName("Salvage Award Claimed")]
    public bool SalvageAwardClaimed { get; }

    [DisplayName("Services Description")]
    public string? ServicesDescription { get; }

    [DisplayName("Services Duration")]
    public string? ServicesDuration { get; } //Units? Unknown.

    [DisplayName("Services Estimated Cost")]

    public double? ServicesEstimatedCost { get; }

    [DisplayName("MMO Licence Required")]
    public bool MmoLicenceRequired { get; }

    [DisplayName("MMO Licence Provided")]
    public bool MmoLicenceProvided { get; }

    [DisplayName("Salvage Claim Awarded")]
    public double SalvageClaimAwarded { get; }

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
    public string? RecoveredFromLegacy { get; }

    [DisplayName("Imported From Legacy")]
    public bool ImportedFromLegacy { get; }
    
    [DisplayName("Legacy Remarks")]
    [DataType(DataType.MultilineText)]
    public string? LegacyRemarks { get; }
}
