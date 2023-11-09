using Droits.Models.Entities;

namespace Droits.Models.DTOs;

public class SalvorDto
{
    public SalvorDto()
    {
        
    }


    public SalvorDto(Salvor salvor)
    {
        Id = salvor.Id;
        Created = salvor.Created;
        LastModified = salvor.LastModified;
        Name = salvor.Name;
        Email = salvor.Email;
    }
    public Guid? Id { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? LastModified { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }

}