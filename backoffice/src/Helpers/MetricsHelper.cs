using System.Globalization;
using Droits.Helpers.Extensions;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Helpers;

public static class MetricsHelper
{
  public static IEnumerable<object>? GetDroitsMetrics(List<Droit> droits)
    {
        var allStatuses = Enum.GetValues(typeof(DroitStatus)).Cast<DroitStatus>().ToList();

        // Find the earliest and latest years
        var minYear = droits.Min(d => d.ReportedDate.Year);
        var maxYear = DateTime.Now.Year;

        var groupedDroits = new List<object>
        {
            new
            {
                Year = "Total",
                Groups = GetGroupedDroits(droits, allStatuses)
            }
        };

        groupedDroits.AddRange(Enumerable.Range(minYear, maxYear - minYear + 1).Reverse()
            .Select(year => new
            {
                Year = year,
                Groups = GetGroupedDroits(droits.Where(d => d.ReportedDate.Year == year), allStatuses, year)
            }));

        return groupedDroits;
    }

    private static IEnumerable<object> GetGroupedDroits(IEnumerable<Droit> droits, IEnumerable<DroitStatus> allStatuses, int year = 0)
    {
        var groupedDroits = new List<object>
        {
            // Group by total
            new
            {
                Group = "Total",
                CountPerStatus = GetCountPerStatus(droits, allStatuses),
                CountPerTriage = GetCountPerTriage(droits)
            }
        };

        // Group by months
        if ( year == 0 ) return groupedDroits;
        
        var months = year < DateTime.Now.Year
            ? Enumerable.Range(1, 12)
            : Enumerable.Range(1, DateTime.Now.Month);
            
        groupedDroits.AddRange(months.Reverse().Select(month => new
        {
            Group = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
            CountPerStatus = GetCountPerStatus(droits.Where(d => d.ReportedDate.Year == year && d.ReportedDate.Month == month), allStatuses),
            CountPerTriage = GetCountPerTriage(droits.Where(d => d.ReportedDate.Year == year && d.ReportedDate.Month == month))
        }));

        return groupedDroits;
    }

    private static Dictionary<string, int> GetCountPerStatus(IEnumerable<Droit> droits, IEnumerable<DroitStatus> allStatuses)
    {
        return allStatuses
            .Select(status => new
            {
                Status = $"{status.GetDisplayName()} Count",
                Count = droits.Count(d => d.Status == status)
            })
            .ToDictionary(x => x.Status, x => x.Count)
            .Concat(new Dictionary<string, int>
            {
                { "Open Count", droits.Count(d => d.Status != DroitStatus.Closed) },
                { "Reported Count", droits.Count() }
            })
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private static Dictionary<string, int> GetCountPerTriage(IEnumerable<Droit> droits)
    {
        return Enumerable.Range(1, 5)
            .Select(triageNumber => new
            {
                TriageNumber = $"Triage {triageNumber} Count",
                Count = droits.Count(d => d.TriageNumber == triageNumber)
            })
            .ToDictionary(x => x.TriageNumber, x => x.Count);
    }
}