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
        Line1 = address.Line1;
        Line2 = address.Line2;
        Town = address.Town;
        County = address.County;
        Postcode = address.Postcode;
    }
    public string Line1 { get; set; } = string.Empty;
    public string Line2 { get; set; } = string.Empty;
    public string Town { get; set; } = string.Empty;
    public string County { get; set; } = string.Empty;
    [Required(ErrorMessage = "Postcode is required")]
    [RegularExpression("([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})" +
                       "|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([AZa-z][0-9][A-Za-z])" +
                       "|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))[0-9][A-Za-z]{2})",
        ErrorMessage = "Invalid postcode")]
    public string Postcode { get; set; } = string.Empty;
}