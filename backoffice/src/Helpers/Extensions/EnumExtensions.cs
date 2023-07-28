using System.ComponentModel.DataAnnotations;

namespace Droits.Helpers.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

        if ( memberInfo?.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() is
            DisplayAttribute displayAttribute )
            return displayAttribute.Name ?? string.Empty;

        return enumValue.ToString();
    }
}