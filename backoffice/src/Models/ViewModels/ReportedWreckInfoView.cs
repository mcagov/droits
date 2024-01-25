#region

using Droits.Models.Entities;
using Droits.Models.Enums;

#endregion

namespace Droits.Models.ViewModels;

public class ReportedWreckInfoView
{
    public ReportedWreckInfoView()
    {
        
    }


    public ReportedWreckInfoView(Droit droit)
    {
        ReportedWreckName = droit.ReportedWreckName;
        ReportedWreckConstructionDetails = droit.ReportedWreckConstructionDetails;
        ReportedWreckYearConstructed = droit.ReportedWreckYearConstructed;
        ReportedWreckYearSunk = droit.ReportedWreckYearSunk;
        
        Latitude = droit.Latitude;
        Longitude = droit.Longitude;
        LocationRadius = droit.LocationRadius;
        Depth = droit.Depth;
        LocationDescription = droit.LocationDescription;

        RecoveredFrom = droit.RecoveredFrom;

    }
    
    public string? ReportedWreckName { get; set; }
    public int? ReportedWreckYearSunk { get; set; }
    public int? ReportedWreckYearConstructed { get; set; }
    public string? ReportedWreckConstructionDetails { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public int? LocationRadius { get; set; }

    public int? Depth { get; set; }
    
    public RecoveredFrom? RecoveredFrom { get; set; }
    public string? LocationDescription { get; set; } = string.Empty;

}