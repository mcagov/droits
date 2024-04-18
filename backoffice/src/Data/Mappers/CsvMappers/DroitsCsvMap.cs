using System.Collections;
using System.Globalization;
using AutoMapper;
using CsvHelper.Configuration;
using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.FormModels.ExportFormModels;

namespace Droits.Data.Mappers.CsvMappers;

public sealed class DroitsCsvMap : ClassMap<DroitExportDto>
{
    public DroitsCsvMap(DroitExportForm droitExportForm)
    {
        AutoMap(CultureInfo.InvariantCulture);
        if ( !droitExportForm.Id )
            Map(d => d.Id).Ignore();
        if ( !droitExportForm.Reference )
            Map(d => d.Reference).Ignore();
        if ( !droitExportForm.Created )
            Map(d => d.Created).Ignore();
        if ( !droitExportForm.LastModified )
            Map(d => d.LastModified).Ignore();
        if ( !droitExportForm.WreckName )
            Map(d => d.WreckName).Ignore();
        if ( !droitExportForm.SalvorName )
            Map(d => d.SalvorName).Ignore();
        if ( !droitExportForm.AssignedTo )
            Map(d => d.AssignedTo).Ignore();
        if ( !droitExportForm.Status )
            Map(d => d.Status).Ignore();
        if ( !droitExportForm.TriageNumber )
            Map(d => d.TriageNumber).Ignore();
        if ( !droitExportForm.ReportedDate )
            Map(d => d.ReportedDate).Ignore();
        if ( !droitExportForm.ClosedDate )
            Map(d => d.ClosedDate).Ignore();
        if ( !droitExportForm.StatutoryDeadline )
            Map(d => d.StatutoryDeadline).Ignore();
        if ( !droitExportForm.DaysTakenToReport )
            Map(d => d.DaysTakenToReport).Ignore();
        if ( !droitExportForm.DateFound )
            Map(d => d.DateFound).Ignore();
        if ( !droitExportForm.WreckMaterialOutcomes )
            Map(d => d.WreckMaterialOutcomes).Ignore();
        if ( !droitExportForm.IsHazardousFind )
            Map(d => d.IsHazardousFind).Ignore();
        if ( !droitExportForm.IsDredge )
            Map(d => d.IsDredge).Ignore();
        if ( !droitExportForm.ReportedWreckName )
            Map(d => d.ReportedWreckName).Ignore();
        if ( !droitExportForm.ReportedWreckYearSunk )
            Map(d => d.ReportedWreckYearSunk).Ignore();
        if ( !droitExportForm.ReportedWreckYearConstructed )
            Map(d => d.ReportedWreckYearConstructed).Ignore();
        if ( !droitExportForm.ReportedWreckConstructionDetails )
            Map(d => d.ReportedWreckConstructionDetails).Ignore();
        if ( !droitExportForm.Latitude )
            Map(d => d.Latitude).Ignore();
        if ( !droitExportForm.Longitude )
            Map(d => d.Longitude).Ignore();
        if ( !droitExportForm.InUkWaters )
            Map(d => d.InUkWaters).Ignore();
        if ( !droitExportForm.LocationRadius )
            Map(d => d.LocationRadius).Ignore();
        if ( !droitExportForm.Depth )
            Map(d => d.Depth).Ignore();
        if ( !droitExportForm.RecoveredFrom )
            Map(d => d.RecoveredFrom).Ignore();
        if ( !droitExportForm.LocationDescription )
            Map(d => d.LocationDescription).Ignore();
        if ( !droitExportForm.SalvageAwardClaimed )
            Map(d => d.SalvageAwardClaimed).Ignore();
        if ( !droitExportForm.ServicesDescription )
            Map(d => d.ServicesDescription).Ignore();
        if ( !droitExportForm.ServicesDuration )
            Map(d => d.ServicesDuration).Ignore();
        if ( !droitExportForm.ServicesEstimatedCost )
            Map(d => d.ServicesEstimatedCost).Ignore();
        if ( !droitExportForm.MmoLicenceRequired )
            Map(d => d.MmoLicenceRequired).Ignore();
        if ( !droitExportForm.MmoLicenceProvided )
            Map(d => d.MmoLicenceProvided).Ignore();
        if ( !droitExportForm.SalvageClaimAwarded )
            Map(d => d.SalvageClaimAwarded).Ignore();
        if ( !droitExportForm.District )
            Map(d => d.District).Ignore();
        if ( !droitExportForm.LegacyFileReference )
            Map(d => d.LegacyFileReference).Ignore();
        if ( !droitExportForm.GoodsDischargedBy )
            Map(d => d.GoodsDischargedBy).Ignore();
        if ( !droitExportForm.DateDelivered )
            Map(d => d.DateDelivered).Ignore();
        if ( !droitExportForm.Agent )
            Map(d => d.Agent).Ignore();
        if ( !droitExportForm.RecoveredFromLegacy )
            Map(d => d.RecoveredFromLegacy).Ignore();
        if ( !droitExportForm.LegacyRemarks )
            Map(d => d.LegacyRemarks).Ignore();
        if ( !droitExportForm.ImportedFromLegacy )
            Map(d => d.ImportedFromLegacy).Ignore();
        if ( droitExportForm.WreckMaterials )
            Map(d => d.WreckMaterials);

    }

}