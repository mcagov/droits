#region

using System.ComponentModel;
using Droits.Helpers.Extensions;
using Droits.Models.Entities;

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
        }

        public Guid? Id { get; set; }
        public string? Reference { get; set; }
        public string? Created { get; set; }
        [DisplayName("Last Modified")]
        public string? LastModified { get; set; }
        [DisplayName("Wreck Name")]
        public string? WreckName { get; set; }
        [DisplayName("Salvor Name")]
        public string? SalvorName { get; set; }
        [DisplayName("Assigned To")]
        public string? AssignedTo { get; set; }
        public string? Status { get; set; }
    }
}