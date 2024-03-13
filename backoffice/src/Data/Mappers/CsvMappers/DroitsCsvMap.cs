using System.Globalization;
using CsvHelper.Configuration;
using Droits.Models.DTOs.Exports;
using Droits.Models.FormModels;

namespace Droits.Data.Mappers.CsvMappers;

public sealed class DroitsCsvMap : ClassMap<DroitExportDto>
{
    public DroitsCsvMap(ExportFieldsForm exportFieldsForm)
    {
        AutoMap(CultureInfo.InvariantCulture);
        if (!exportFieldsForm.Id)
            Map(d => d.Id).Ignore();
        if (!exportFieldsForm.Reference)
            Map(d => d.Reference).Ignore();
        if (!exportFieldsForm.Created)
            Map(d => d.Created).Ignore();
        if (!exportFieldsForm.LastModified)
            Map(d => d.LastModified).Ignore();
        if (!exportFieldsForm.WreckName)
            Map(d => d.WreckName).Ignore();
        if (!exportFieldsForm.SalvorName)
            Map(d => d.SalvorName).Ignore();
        if (!exportFieldsForm.AssignedTo)
            Map(d => d.AssignedTo).Ignore();
        if (!exportFieldsForm.Status)
            Map(d => d.Status).Ignore();
        if (!exportFieldsForm.TriageNumber)
            Map(d => d.TriageNumber).Ignore();
        if (!exportFieldsForm.ReportedDate)
            Map(d => d.ReportedDate).Ignore();
        if (!exportFieldsForm.DateFound)
            Map(d => d.DateFound).Ignore();
        if (!exportFieldsForm.IsHazardousFind)
            Map(d => d.IsHazardousFind).Ignore();
        if (!exportFieldsForm.IsDredge)
            Map(d => d.IsDredge).Ignore();
        if (!exportFieldsForm.ReportedWreckName)
            Map(d => d.ReportedWreckName).Ignore();
        if (!exportFieldsForm.ReportedWreckYearSunk)
            Map(d => d.ReportedWreckYearSunk).Ignore();
        if (!exportFieldsForm.ReportedWreckYearConstructed)
            Map(d => d.ReportedWreckYearConstructed).Ignore();
        if (!exportFieldsForm.ReportedWreckConstructionDetails)
            Map(d => d.ReportedWreckConstructionDetails).Ignore();
        if (!exportFieldsForm.Latitude)
            Map(d => d.Latitude).Ignore();
        if (!exportFieldsForm.Longitude)
            Map(d => d.Longitude).Ignore();
        if (!exportFieldsForm.InUkWaters)
            Map(d => d.InUkWaters).Ignore();
        if (!exportFieldsForm.LocationRadius)
            Map(d => d.LocationRadius).Ignore();
        if (!exportFieldsForm.Depth)
            Map(d => d.Depth).Ignore();
        if (!exportFieldsForm.RecoveredFrom)
            Map(d => d.RecoveredFrom).Ignore();
        if (!exportFieldsForm.LocationDescription)
            Map(d => d.LocationDescription).Ignore();
        if (!exportFieldsForm.SalvageAwardClaimed)
            Map(d => d.SalvageAwardClaimed).Ignore();
        if (!exportFieldsForm.ServicesDescription)
            Map(d => d.ServicesDescription).Ignore();
        if (!exportFieldsForm.ServicesDuration)
            Map(d => d.ServicesDuration).Ignore();
        if (!exportFieldsForm.ServicesEstimatedCost)
            Map(d => d.ServicesEstimatedCost).Ignore();
        if (!exportFieldsForm.MmoLicenceRequired)
            Map(d => d.MmoLicenceRequired).Ignore();
        if (!exportFieldsForm.MmoLicenceProvided)
            Map(d => d.MmoLicenceProvided).Ignore();
        if (!exportFieldsForm.SalvageClaimAwarded)
            Map(d => d.SalvageClaimAwarded).Ignore();
        if (!exportFieldsForm.District)
            Map(d => d.District).Ignore();
        if (!exportFieldsForm.LegacyFileReference)
            Map(d => d.LegacyFileReference).Ignore();
        if (!exportFieldsForm.GoodsDischargedBy)
            Map(d => d.GoodsDischargedBy).Ignore();
        if (!exportFieldsForm.DateDelivered)
            Map(d => d.DateDelivered).Ignore();
        if (!exportFieldsForm.Agent)
            Map(d => d.Agent).Ignore();
        if (!exportFieldsForm.RecoveredFromLegacy)
            Map(d => d.RecoveredFromLegacy).Ignore();
        if (!exportFieldsForm.LegacyRemarks)
            Map(d => d.LegacyRemarks).Ignore();
        if (!exportFieldsForm.ImportedFromLegacy)
            Map(d => d.ImportedFromLegacy).Ignore();
        
    }
}