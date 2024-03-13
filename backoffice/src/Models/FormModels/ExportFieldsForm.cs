using System.ComponentModel;
using Droits.Models.Enums;

namespace Droits.Models.FormModels;

public class ExportFieldsForm
{
    public ExportFieldsForm()
    {
        
    }
    
    
    public bool Id { get; set; }
    public bool Reference { get; set; }
    public bool Created { get; set; }
    [DisplayName("Last Modified")]
    public bool LastModified { get; set; }
    [DisplayName("Verified Wreck Name")]
    public bool WreckName { get; set; }
    [DisplayName("Salvor Name")]
    public bool SalvorName { get; set; }
    [DisplayName("Assigned To")]
    public bool AssignedTo { get; set; }
    public bool Status { get; set; }
    
    [DisplayName("Triage Number")]
    public bool TriageNumber { get; set; }
    
    [DisplayName("Reported Date")]
    public bool ReportedDate { get; set; }

    [DisplayName("Date Found")]
    public bool DateFound { get; set; }

    [DisplayName("Is Hazardous Find?")]
    public bool IsHazardousFind { get; set; }
    
    [DisplayName("Is Dredge?")]
    public bool IsDredge { get; set; }

    [DisplayName("Reported Wreck Name")]
    public bool ReportedWreckName { get; set; }

    [DisplayName("Reported Wreck Year Sunk")]
    public bool ReportedWreckYearSunk { get; set; }

    [DisplayName("Reported Wreck Year Constructed")]
    public bool ReportedWreckYearConstructed { get; set; }

    [DisplayName("Reported Wreck Construction Details")]
    public bool ReportedWreckConstructionDetails { get; set; }

    public bool Latitude { get; set; }
    
    public bool Longitude { get; set; }
        
    [DisplayName("in Uk Waters?")]
    public bool InUkWaters { get; set; }

    [DisplayName("Location Radius")]
    public bool LocationRadius { get; set; } 

    public bool Depth { get; set; } 
    
    [DisplayName("Recovered From")]
    public bool RecoveredFrom { get; set; }

    [DisplayName("Location Description")]
    public bool LocationDescription { get; set; }

    [DisplayName("Salvage Award Claimed")]
    public bool SalvageAwardClaimed { get; set; }

    [DisplayName("Services Description")]
    public bool ServicesDescription { get; set; }
    
    [DisplayName("Services Duration")]
    public bool ServicesDuration { get; set; }

    [DisplayName("Services Estimated Cost")]
    public bool ServicesEstimatedCost { get; set; }

    [DisplayName("Mmo Licence Required")]
    public bool MmoLicenceRequired { get; set; }

    [DisplayName("Mmo Licence Required")]
    public bool MmoLicenceProvided { get; set; }

    [DisplayName("Salvage Claim Awarded")]
    public bool SalvageClaimAwarded { get; set; }

    [DisplayName("District")]
    public bool District { get; set; }

    [DisplayName("Legacy File Reference")]
    public bool LegacyFileReference { get; set; } 

    [DisplayName("Goods Discharged By")]
    public bool GoodsDischargedBy { get; set; } 

    [DisplayName("Date Delivered")]
    public bool DateDelivered { get; set; } 

    [DisplayName("Agent")]
    public bool Agent { get; set; }

    [DisplayName("Recovered from Legacy")]
    public bool RecoveredFromLegacy { get; set; }

    [DisplayName("Legacy Remarks")]
    public bool LegacyRemarks { get; set; }

    [DisplayName("Imported from Legacy")]
    public bool ImportedFromLegacy { get; set; }
}