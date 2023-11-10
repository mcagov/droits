#region

using System.ComponentModel;
using Droits.Models.Entities;

#endregion

namespace Droits.Models.DTOs;

public class SalvorExportDto
{
    public SalvorExportDto()
    {
        
    }


    public SalvorExportDto(Salvor salvor)
    {
        Created = salvor.Created;
        LastModified = salvor.LastModified;
        Name = salvor.Name;
        Email = salvor.Email;
        TelephoneNumber = salvor.TelephoneNumber;
        AddressLine1 = salvor.Address.Line1;
        AddressLine2 = salvor.Address.Line2;
        AddressTown = salvor.Address.Town;
        AddressCounty = salvor.Address.County;
        AddressPostcode = salvor.Address.Postcode;

    }

    public DateTime? Created { get; set; }
    [DisplayName("Last Modified")]
    public DateTime? LastModified { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    [DisplayName("Telephone Number")]
    public string? TelephoneNumber { get; set; } 
    [DisplayName("Address Postcode")]
    public string AddressPostcode { get; set; }
    [DisplayName("Address County")]
    public string AddressCounty { get; set; }
    [DisplayName("Address Town")]
    public string AddressTown { get; set; }
    [DisplayName("Address Line One")]
    public string AddressLine1 { get; set; }
    [DisplayName("Address Line Two")]
    public string? AddressLine2 { get; set; }
        

}