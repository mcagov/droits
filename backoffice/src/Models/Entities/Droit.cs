#region

using System.ComponentModel.DataAnnotations.Schema;
using Droits.Helpers.Extensions;
using Droits.Models.Enums;

#endregion

namespace Droits.Models.Entities;

public class Droit : BaseEntity
{

    public Guid? AssignedToUserId { get; set; }
    
    [ForeignKey("AssignedToUserId")]
    public virtual ApplicationUser? AssignedToUser { get; set; } = null!;
    
    public string Reference { get; set; } = string.Empty;
    public DroitStatus Status { get; set; } = DroitStatus.Received;
    
    public int? TriageNumber { get; set; }
    public DateTime ReportedDate { get; set; } = DateTime.UtcNow;
    public DateTime DateFound { get; set; } = DateTime.UtcNow;
    public int DaysTakenToReport => DateFound.DaysBetween(ReportedDate);
    public string OriginalSubmission { get; set; } = string.Empty;
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    public virtual ICollection<WreckMaterial> WreckMaterials { get; set; } = new List<WreckMaterial>();
    public virtual ICollection<Letter> Letters { get; set; } = new List<Letter>();
 

    // Wreck

    [ForeignKey("WreckId")]
    public Wreck? Wreck { get; set; }

    public Guid? WreckId { get; set; }
    public bool IsHazardousFind { get; set; } = false;
    public bool IsDredge { get; set; } = false;
    
    //Reported Wreck 
    
    public string? ReportedWreckName { get; set; }
    public int? ReportedWreckYearSunk { get; set; }
    public int? ReportedWreckYearConstructed { get; set; }
    public string? ReportedWreckConstructionDetails { get; set; }



    // Salvor

    [ForeignKey("SalvorId")]
    public Salvor? Salvor { get; set; }

    public Guid? SalvorId { get; set; }

    // Location
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool InUkWaters { get; set; } = false;
    public int? LocationRadius { get; set; } //Units? Unknown.
    public int? Depth { get; set; } //Units? Unknown. //metres from frontend
    public RecoveredFrom? RecoveredFrom { get; set; } 
    public string? LocationDescription { get; set; } = string.Empty;


    // Salvage

    public bool SalvageAwardClaimed { get; set; }
    public string? ServicesDescription { get; set; }
    public string? ServicesDuration { get; set; } //Units? Unknown.
    public double? ServicesEstimatedCost { get; set; }
    public bool MmoLicenceRequired { get; set; }
    public bool MmoLicenceProvided { get; set; }
    public double SalvageClaimAwarded { get; set; }
    
    // Legacy fields
    public string? PowerappsDroitId { get; set; }
    public string? PowerappsWreckId { get; set; }
    public string? District { get; set; } 
    public string? LegacyFileReference { get; set; } //Physical file location/ref
    public string? GoodsDischargedBy { get; set; } //Initials of RoW member.
    public string? DateDelivered { get; set; } //Unsure of date format.
    public string? Agent { get; set; }
    public string? RecoveredFromLegacy { get; set; }
    public string? LegacyRemarks { get; set; }
    public bool ImportedFromLegacy { get; set; } = false;
}