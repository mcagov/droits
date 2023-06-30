using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

namespace Droits.Models.FormModels;

public class AddressForm
{
    public AddressForm()
    {
    }

    public AddressForm(Address address)
    {
        AddressLine1 = address.Line1;
        AddressLine2 = address.Line2;
        AddressTown = address.Town;
        AddressCounty = address.County;
        AddressPostcode = address.Postcode;
    }
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string AddressTown { get; set; } = string.Empty;
    public string AddressCounty { get; set; } = string.Empty;
    [Required(ErrorMessage = "Postcode is required")]
    [RegularExpression("([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})" +
                       "|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([AZa-z][0-9][A-Za-z])" +
                       "|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))[0-9][A-Za-z]{2})",
        ErrorMessage = "Invalid postcode")]
    public string AddressPostcode { get; set; } = string.Empty;
}