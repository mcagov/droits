namespace Droits.Helpers.Extensions;

public static class DateTimeExtensions
{
    public static bool IsBetween(this DateTime date, DateTime? from, DateTime? to)
    {
        return ( !from.HasValue || date >= StartOfDay(from.Value) ) &&
          ( !to.HasValue || date <= EndOfDay(to.Value) ) ;
    }

    private static DateTime EndOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
    }
    
    private static DateTime StartOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 00, 00, 00, 000);
    }
}