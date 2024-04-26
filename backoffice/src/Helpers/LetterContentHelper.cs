using System.Text.RegularExpressions;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs;
using Droits.Models.Entities;

namespace Droits.Helpers;

public static class LetterContentHelper
{ 
    
    public static string GetReportConfirmedEmailBodyAsync(Droit droit, string templateBody)
    {

        var submittedWreckName = droit.ReportedWreckName.ValueOrEmpty();

        var itemsReportedSection = BuildItemsReportedSection(droit, submittedWreckName ?? string.Empty);

        templateBody = Regex.Replace(templateBody, @"\(\(items_reported_section\)\)", itemsReportedSection);

        var isLateSubmission = droit.ReportedDate.Subtract(droit.DateFound).Days > 28;
        var lateReportSection = BuildLateReportSection(isLateSubmission);

        templateBody = Regex.Replace(templateBody, @"\(\(late_report_section\)\)", lateReportSection);

        var yourResponsibilitiesSection = BuildYourResponsibilitiesSection(droit.WreckMaterials.Count);

        templateBody = Regex.Replace(templateBody, @"\(\(your_responsibilities_section\)\)", yourResponsibilitiesSection);

        return templateBody;
    }

    private static string BuildItemsReportedSection(Droit droit, string submittedWreckName)
    {
        var itemsReportedSection = $@"
            The following {"item has".Pluralize(droit.WreckMaterials.Count, "items have")} been reported {(!string.IsNullOrEmpty(submittedWreckName) ? $"from the wreck named {submittedWreckName}" : "")} :
    ";
        itemsReportedSection += " \n";
        itemsReportedSection = droit.WreckMaterials.Aggregate(itemsReportedSection, (current, item) => current + $"\n - {item.Name}");

        return itemsReportedSection;
    }

    private static string BuildLateReportSection(bool isLateSubmission)
    {
        var lateReportSection = "";

        if ( !isLateSubmission ) return lateReportSection;
        
        lateReportSection += @"#Report submitted late";
        lateReportSection += @"
                You have not submitted this Report of wreck material within 28 days of the wreck material being recovered. For all future recoveries, please note that all recovered wreck material should be reported to the Receiver of Wreck within 28 days of recovery.";

        return lateReportSection;
    }

    private static string BuildYourResponsibilitiesSection(int itemCount)
    {
        var yourResponsibilitiesSection = $@"
                The recovered {"item is".Pluralize(itemCount, "items are")} now held by you on indemnity to the Receiver of Wreck whilst investigations into legal ownership are undertaken.

                You do not own the reported {"item".Pluralize(itemCount, "items")} until advised by the Receiver of Wreck. You must not sell or otherwise dispose of the {"item".Pluralize(itemCount, "items")} until advised by the Receiver of Wreck.

                All finds from a maritime context are subject to various physical and chemical processes once raised. Conservators advise that finds should be kept wet, cool and dark until more specific advice can be sought.
    ";

        return yourResponsibilitiesSection;
    }
}