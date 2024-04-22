using System.Globalization;
using Droits.Models.Enums;

namespace Droits.Helpers.Extensions;

public static class StringExtensions
{
    public static bool HasValue(this string? value)
    {
        return !string.IsNullOrEmpty(value);
    }


    public static string? ValueOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value) ? string.Empty : value;
    }


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

    public static string Pluralize(this string? word, int count, string? pluralForm = null)
    {
        if ( string.IsNullOrEmpty(word) )
        {
            return string.Empty;
        }
        return count == 1
            ? word
            : pluralForm ?? (word.EndsWith("y", StringComparison.OrdinalIgnoreCase) ? string.Concat(word.AsSpan(0, word.Length - 1), "ies") : word + "s");
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
    
    
    public static string ConvertToProperCase(this string input) =>
        string.IsNullOrEmpty(input) ? input : char.ToUpper(input[0]) +
                                              string.Concat(input.Skip(1).Select((x, i) => char.IsUpper(x) ? " " + x : x.ToString()));

}