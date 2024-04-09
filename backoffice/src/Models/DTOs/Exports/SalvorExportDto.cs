#region

using System.ComponentModel;
using Droits.Models.Entities;

#endregion

namespace Droits.Models.DTOs.Exports;

public class SalvorExportDto
{
    
    public SalvorExportDto(Salvor salvor)
    {
        Id = salvor.Id;
        Created = salvor.Created;
        LastModified = salvor.LastModified;
        Name = salvor.Name;
        Email = salvor.Email;
        TelephoneNumber = salvor.TelephoneNumber;
        MobileNumber = salvor.MobileNumber;
        AddressLine1 = salvor.Address.Line1;
        AddressLine2 = salvor.Address.Line2;
        AddressTown = salvor.Address.Town;
        AddressCounty = salvor.Address.County;
        AddressPostcode = salvor.Address.Postcode;
        DroitCount = salvor.Droits.Count.ToString();
        DroitRefs = string.Join(", ",salvor.Droits.Select(droit => droit.Reference).ToList());
    }


    public Guid Id { get; set; }

    public DateTime? Created { get; set; }
    [DisplayName("Last Modified")]
    public DateTime? LastModified { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    [DisplayName("Telephone Number")]
    public string? TelephoneNumber { get; set; } 
    
    [DisplayName("Mobile Number")]
    public string? MobileNumber { get; set; } 
    [DisplayName("Address Postcode")]
    public string? AddressPostcode { get; set; }
    [DisplayName("Address County")]
    public string? AddressCounty { get; set; }
    [DisplayName("Address Town")]
    public string? AddressTown { get; set; }
    [DisplayName("Address Line One")]
    public string? AddressLine1 { get; set; }
    [DisplayName("Address Line Two")]
    public string? AddressLine2 { get; set; }
    [DisplayName("Droit Count")]
    public string? DroitCount { get; set; }
    [DisplayName("Droit References")]
    public string? DroitRefs { get; set; }
}