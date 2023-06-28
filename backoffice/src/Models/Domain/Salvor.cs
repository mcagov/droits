using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Droits.Models;

public class Salvor
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string TelephoneNumber { get; set; } = string.Empty;
    public Address Address { get; set; } = new();
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; } 
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
}