using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Helpers;
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


    [Required]
    [DisplayName("Line 1")]
    public string Line1 { get; set; } = string.Empty;


    [DisplayName("Line 2")]
    public string? Line2 { get; set; } = string.Empty;

    [Required]
    [DisplayName("City/Town")]
    public string Town { get; set; } = string.Empty;

    public string? County { get; set; } = string.Empty;

    [Required(ErrorMessage = "Postcode is required")]
    [RegularExpression(Constants.PostcodeRegex,
        ErrorMessage = "Invalid postcode")]
    public string Postcode { get; set; } = string.Empty;


    public Address ApplyChanges(Address address)
    {
        address.Line1 = Line1;
        address.Line2 = Line2;
        address.Town = Town;
        address.County = County;
        address.Postcode = Postcode;

        return address;
    }
}