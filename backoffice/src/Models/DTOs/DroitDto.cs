using Droits.Models.Entities;

namespace Droits.Models.DTOs;

public class DroitDto
{
   
    public DroitDto(Droit droit)
    {

        if ( droit == null )
        {
            throw new ArgumentNullException();
        }
        
        Id = droit.Id;
        Reference = droit.Reference;

        Created = droit.Created.ToString("dd/MM/yyyy");;
        LastModified = droit.LastModified.ToString("dd/MM/yyyy");;

        if ( droit.WreckId.HasValue && droit.WreckId != default )
        {
            WreckId = droit.WreckId?.ToString() ?? string.Empty;    
        }
        
        WreckName = droit?.Wreck?.Name ?? "No Wreck";
        
        if ( droit.SalvorId.HasValue && droit.SalvorId != default )
        {
            SalvorId = droit.SalvorId?.ToString() ?? string.Empty;    
        }

        
        SalvorName = droit?.Salvor?.Name ?? "Unknown";
        AssignedTo = droit?.AssignedToUser?.Name ?? "Unassigned";

    }
    public Guid Id { get; set; }
    public string Reference { get; set; }
    public string Created { get; set; }
    public string LastModified { get; set; }
    public string WreckName { get; set; }
    public string WreckId { get; set; }
    public string SalvorName { get; set; }
    public string SalvorId { get; set; }
    public string AssignedTo { get; set; }
    
    
}