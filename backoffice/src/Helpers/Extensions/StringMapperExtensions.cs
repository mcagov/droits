using System.Globalization;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Helpers.Extensions;

public static class StringMapperExtensions
{
    
    public static bool AsBoolean(this string? value)
    {
        if ( string.IsNullOrEmpty(value) )
        {
            return false;
        }

        value = value.ToLower().Trim();
        
        return value.StartsWith("y") || value.StartsWith("t");
    }
    
    public static DateTime? AsDateTime(this string? dateString)
    {

        if ( string.IsNullOrEmpty(dateString) )
        {
            return null; 
        }
        
        if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", null, DateTimeStyles.None, out var fixedFormatResult))
        {
            return fixedFormatResult;
        }

        if (DateTime.TryParse(dateString, null, DateTimeStyles.None, out var anyFormatResult))
        {
            return anyFormatResult;
        }
        return null;
    }

    public static int? AsInt(this string? intString)
    {
        if (int.TryParse(intString, out var number))
        {
            return number;
        }

        return null;
    }
    
    public static double? AsDouble(this string? doubleString)
    {
        if (double.TryParse(doubleString, out var number))
        {
            return number;
        }

        return null;
    }

    public static RecoveredFrom? AsRecoveredFromEnum(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalizedValue = value.Replace(" ", "").ToLower();

        return normalizedValue switch
        {
            "shipwreck" => RecoveredFrom.Shipwreck,
            "seabed" => RecoveredFrom.Seabed,
            "afloat" => RecoveredFrom.Afloat,
            "seashore" => RecoveredFrom.SeaShore,
            _ => null
        };
    }
    
    public static WreckMaterialOutcome? AsWreckMaterialOutcomeEnum(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalizedValue = value.Replace(" ", "").ToLower();

        return normalizedValue switch
        {
            "lieuofsalvage" => WreckMaterialOutcome.LieuOfSalvage,
            "returnedtoowner" => WreckMaterialOutcome.ReturnedToOwner,
            "donatedtomuseum" => WreckMaterialOutcome.DonatedToMuseum,
            "soldtomuseum" => WreckMaterialOutcome.SoldToMuseum,
            _ => WreckMaterialOutcome.Other
        };
    }
    
    public static Address? AsAddress(this string? addressString)
    {
        if (string.IsNullOrEmpty(addressString))
        {
            return null;
        }

        var lines = addressString.Split('\n')
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrEmpty(line))
            .ToList();

        return new Address
        {
            Line1 = lines.Count > 0 ? lines[0] : string.Empty,
            Line2 = lines.Count > 1 ? lines[1] : string.Empty,
            Town = lines.Count > 2 ? lines[2] : string.Empty,
            County = lines.Count > 3 ? lines[3] : string.Empty,
            Postcode = lines.Count > 4 ? lines[4] : string.Empty
        };
    }
    
    
}