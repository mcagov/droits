using System.ComponentModel;
using Droits.Models.Enums;

namespace Droits.Models.FormModels;

public class DroitExportForm
{
    public DroitExportForm()
    {
        
    }


    public bool Id { get; set; }
    public bool Reference { get; set; } = true;
    public bool Created { get; set; } = true;
    [DisplayName("Last Modified")]
    public bool LastModified { get; set; } = true;
    [DisplayName("Verified Wreck Name")]
    public bool WreckName { get; set; } = true;
    [DisplayName("Salvor Name")]
    public bool SalvorName { get; set; } = true;
    [DisplayName("Assigned To")]
    public bool AssignedTo { get; set; } = true;
    public bool Status { get; set; } = true;
    
    [DisplayName("Triage Number")]
    public bool TriageNumber { get; set; } = true;
    
    [DisplayName("Reported Date")]
    public bool ReportedDate { get; set; } = true;

    [DisplayName("Date Found")]
    public bool DateFound { get; set; } = true;

    [DisplayName("Is Hazardous Find?")]
    public bool IsHazardousFind { get; set; } = true;
    
    [DisplayName("Is Dredge?")]
    public bool IsDredge { get; set; } = true;
    
    [DisplayName("Wreck Materials")]
    public bool WreckMaterials { get; set; } = true;

    [DisplayName("Reported Wreck Name")]
    public bool ReportedWreckName { get; set; } = true;

    [DisplayName("Reported Wreck Year Sunk")]
    public bool ReportedWreckYearSunk { get; set; } = true;

    [DisplayName("Reported Wreck Year Constructed")]
    public bool ReportedWreckYearConstructed { get; set; } = true;

    [DisplayName("Reported Wreck Construction Details")]
    public bool ReportedWreckConstructionDetails { get; set; } = true;

    public bool Latitude { get; set; } = true;
    
    public bool Longitude { get; set; } = true;
        
    [DisplayName("in Uk Waters?")]
    public bool InUkWaters { get; set; } = true;

    [DisplayName("Location Radius")]
    public bool LocationRadius { get; set; } = true; 

    public bool Depth { get; set; } = true; 
    
    [DisplayName("Recovered From")]
    public bool RecoveredFrom { get; set; } = true;

    [DisplayName("Location Description")]
    public bool LocationDescription { get; set; } = true;

    [DisplayName("Salvage Award Claimed")]
    public bool SalvageAwardClaimed { get; set; } = true;

    [DisplayName("Services Description")]
    public bool ServicesDescription { get; set; } = true;
    
    [DisplayName("Services Duration")]
    public bool ServicesDuration { get; set; } = true;

    [DisplayName("Services Estimated Cost")]
    public bool ServicesEstimatedCost { get; set; } = true;

    [DisplayName("Mmo Licence Required")]
    public bool MmoLicenceRequired { get; set; } = true;

    [DisplayName("Mmo Licence Provided")]
    public bool MmoLicenceProvided { get; set; } = true;

    [DisplayName("Salvage Claim Awarded")]
    public bool SalvageClaimAwarded { get; set; } = true;

    [DisplayName("District")]
    public bool District { get; set; } = true;

    [DisplayName("Legacy File Reference")]
    public bool LegacyFileReference { get; set; } = true; 

    [DisplayName("Goods Discharged By")]
    public bool GoodsDischargedBy { get; set; } = true; 

    [DisplayName("Date Delivered")]
    public bool DateDelivered { get; set; } = true; 

    [DisplayName("Agent")]
    public bool Agent { get; set; } = true;

    [DisplayName("Recovered from Legacy")]
    public bool RecoveredFromLegacy { get; set; } = true;

    [DisplayName("Legacy Remarks")]
    public bool LegacyRemarks { get; set; } = true;

    [DisplayName("Imported from Legacy")]
    public bool ImportedFromLegacy { get; set; } = true;
    


}