#region

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Helpers.Extensions;
using Droits.Models.Entities;
using Droits.Models.Enums;

#endregion

namespace Droits.Models.DTOs.Exports
{
    public class DroitExportDto
    {

        public DroitExportDto()
        {
            
        }
        public DroitExportDto(Droit droit)
        {
            Id = droit.Id;
            Reference = droit.Reference;
            Created = droit.Created.ToString("dd/MM/yyyy");
            LastModified = droit.LastModified.ToString("dd/MM/yyyy");
            WreckName = droit.Wreck?.Name ?? "No Wreck";
            SalvorName = droit.Salvor?.Name ?? "Unknown";
            AssignedTo = droit.AssignedToUser?.Name ?? "Unassigned";
            Status = droit.Status.GetDisplayName();
            TriageNumber = droit.TriageNumber;
            ReportedDate = droit.ReportedDate;
            DateFound = droit.DateFound;
            IsHazardousFind = droit.IsHazardousFind;
            IsDredge = droit.IsDredge;
            ReportedWreckName = droit.ReportedWreckName;
            ReportedWreckYearSunk = droit.ReportedWreckYearSunk;
            ReportedWreckYearConstructed = droit.ReportedWreckYearConstructed;
            ReportedWreckConstructionDetails = droit.ReportedWreckConstructionDetails;
            Latitude = droit.Latitude;
            Longitude = droit.Longitude;
            InUkWaters = droit.InUkWaters;
            LocationRadius = droit.LocationRadius;
            Depth = droit.Depth;
            RecoveredFrom = droit.RecoveredFrom;
            LocationDescription = droit.LocationDescription;
            SalvageAwardClaimed = droit.SalvageAwardClaimed;
            ServicesDescription = droit.ServicesDescription;
            ServicesDuration = droit.ServicesDuration;
            ServicesEstimatedCost = droit.ServicesEstimatedCost;
            MmoLicenceRequired = droit.MmoLicenceRequired;
            MmoLicenceProvided = droit.MmoLicenceProvided;
            SalvageClaimAwarded = droit.SalvageClaimAwarded;
            District = droit.District;
            LegacyFileReference = droit.LegacyFileReference;
            GoodsDischargedBy = droit.GoodsDischargedBy;
            DateDelivered = droit.DateDelivered;
            Agent = droit.Agent;
            RecoveredFromLegacy = droit.RecoveredFromLegacy;
            LegacyRemarks = droit.LegacyRemarks;
            ImportedFromLegacy = droit.ImportedFromLegacy;
        }

        public Guid? Id { get; set; }
        public string? Reference { get; set; }
        public string? Created { get; set; }
        [DisplayName("Last Modified")]
        public string? LastModified { get; set; }
        [DisplayName("Verified Wreck Name")]
        public string? WreckName { get; set; }
        [DisplayName("Salvor Name")]
        public string? SalvorName { get; set; }
        [DisplayName("Assigned To")]
        public string? AssignedTo { get; set; }
        public string? Status { get; set; }
        
        [DisplayName("Triage Number")]
        public int? TriageNumber { get; set; }
        
        [DisplayName("Reported Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReportedDate { get; set; }
    
        [DisplayName("Date Found")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateFound { get; set; }

        [DisplayName("Is Hazardous Find?")]
        public bool IsHazardousFind { get; set; }
        
        [DisplayName("Is Dredge?")]
        public bool IsDredge { get; set; }

        [DisplayName("Reported Wreck Name")]
        public string? ReportedWreckName { get; set; }

        [DisplayName("Reported Wreck Year Sunk")]
        public int? ReportedWreckYearSunk { get; set; }

        [DisplayName("Reported Wreck Year Constructed")]
        public int? ReportedWreckYearConstructed { get; set; }

        [DisplayName("Reported Wreck Construction Details")]
        public string? ReportedWreckConstructionDetails { get; set; }

        public double? Latitude { get; set; }
        
        public double? Longitude { get; set; }
            
        [DisplayName("in Uk Waters?")]
        public bool InUkWaters { get; set; }

        [DisplayName("Location Radius")]
        public int? LocationRadius { get; set; } //Units? Unknown.

        public int? Depth { get; set; } // Metres
        
        [DisplayName("Recovered From")]
        public RecoveredFrom? RecoveredFrom { get; set; }

        [DisplayName("Location Description")]
        public string? LocationDescription { get; set; }

        [DisplayName("Salvage Award Claimed")]
        public bool SalvageAwardClaimed { get; set; }

        [DisplayName("Services Description")]
        public string? ServicesDescription { get; set; }
        
        [DisplayName("Services Duration")]
        public string? ServicesDuration { get; set; }

        [DisplayName("Services Estimated Cost")]
        public double? ServicesEstimatedCost { get; set; }

        [DisplayName("Mmo Licence Required")]
        public bool MmoLicenceRequired { get; set; }

        [DisplayName("Mmo Licence Required")]
        public bool MmoLicenceProvided { get; set; }

        [DisplayName("Salvage Claim Awarded")]
        public double SalvageClaimAwarded { get; set; }

        [DisplayName("District")]
        public string? District { get; set; }

        [DisplayName("Legacy File Reference")]
        public string? LegacyFileReference { get; set; } // Physical file location/ref

        [DisplayName("Goods Discharged By")]
        public string? GoodsDischargedBy { get; set; } // Initials of RoW member.

        [DisplayName("Date Delivered")]
        public string? DateDelivered { get; set; } // Unsure of date format.

        [DisplayName("Agent")]
        public string? Agent { get; set; }

        [DisplayName("Recovered from Legacy")]
        public string? RecoveredFromLegacy { get; set; }

        [DisplayName("Legacy Remarks")]
        public string? LegacyRemarks { get; set; }

        [DisplayName("Imported from Legacy")]
        public bool ImportedFromLegacy { get; set; }
    }
}