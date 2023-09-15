using Droits.Models.Entities;

namespace Droits.Models.DTOs;

public class DroitDto
{
   
    public DroitDto(Droit droit)
    {
        Id = droit.Id;
        Reference = droit.Reference;

        Created = droit.Created.ToString("dd/MM/yyyy");;
        LastModified = droit.LastModified.ToString("dd/MM/yyyy");;

        WreckName = droit?.Wreck?.Name ?? "No Wreck";
        SalvorName = droit?.Salvor?.Name ?? "Unknown";
        AssignedTo = droit?.AssignedToUser?.Name ?? "Unassigned";

    }
    public Guid Id { get; set; }
    public string Reference { get; set; }
    public string Created { get; set; }
    public string LastModified { get; set; }
    public string WreckName { get; set; }
    public string SalvorName { get; set; }
    public string AssignedTo { get; set; }
    
    
}