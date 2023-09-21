using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Droits.Models.FormModels;

public class DroitSearchForm
{
    public DroitSearchForm() 
    {
        
    }


    // Base fields...
    // public WreckForm WreckForm { get; set; } = new();

    [DisplayName("Is Isolated Find")]
    public bool IsIsolatedFind { get; set; } = true;

    // public List<WreckMaterialForm> WreckMaterialForms { get; set; } = new();
    // public SalvorForm SalvorForm { get; set; } = new();

    public string? Reference { get; set; }

    public List<DroitStatus> StatusList { get; set; } = new();
    
    // [DisplayName("Assigned To")]
    // public Guid? AssignedToUserId { get; set; }
    
    [DisplayName("Reported Date From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? ReportedDateFrom { get; set; }

    [DisplayName("Reported Date To")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? ReportedDateTo { get; set; }
    
    [DisplayName("Date Found From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateFoundFrom { get; set; }

    [DisplayName("Date Found To")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateFoundTo{ get; set; }



    // Wreck

    // public Guid? WreckId { get; set; }
    //
    [DisplayName("Is Hazardous Find")]
    public bool IsHazardousFind { get; set; }
    
    [DisplayName("Is Dredge")]
    public bool IsDredge { get; set; }

    // Salvor

    // public Guid? SalvorId { get; set; }


    // Location
     public string? Latitude { get; set; }
     public string? Longitude { get; set; }

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
    public string? RecoveredFromLegacy { get; set; }

    [DisplayName("Imported From Legacy")]
    public bool ImportedFromLegacy { get; set; }

}