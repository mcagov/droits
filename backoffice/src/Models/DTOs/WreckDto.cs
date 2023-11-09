using Droits.Models.Entities;

namespace Droits.Models.DTOs;

public class WreckDto
{
        public WreckDto()
    {
        
    }


    public WreckDto(Wreck wreck)
    {
        Id = wreck.Id;
        Created = wreck.Created;
        LastModified = wreck.LastModified;
        Name = wreck.Name;
    }
    public Guid? Id { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? LastModified { get; set; }
    public string? Name { get; set; }

}