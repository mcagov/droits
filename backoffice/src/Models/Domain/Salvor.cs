using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Droits.Models;

public class Salvor
{
    public Guid Id { get; set; }
    public String Email { get; set; } = string.Empty;
    public String Name { get; set; } = string.Empty;
    public String TelephoneNumber { get; set; } = string.Empty;
    public AddressDetails AddressDetails { get; set; } = new();
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; } 
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
}