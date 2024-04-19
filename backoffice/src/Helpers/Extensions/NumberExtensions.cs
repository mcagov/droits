
namespace Droits.Helpers.Extensions;

public static class NumberExtensions
{
    public static int? AsInt(this double? doubleNumber)
    {
        if (doubleNumber is >= int.MinValue and <= int.MaxValue)
        {
            return (int)Math.Round(doubleNumber.Value);
        }

        return null;
    }
}