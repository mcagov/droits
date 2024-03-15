using System.Collections;
using System.Globalization;
using AutoMapper;
using CsvHelper.Configuration;
using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.FormModels.ExportFormModels;

namespace Droits.Data.Mappers.CsvMappers;

public sealed class WrecksCsvMap : ClassMap<WreckExportDto>
{
    public WrecksCsvMap(WreckExportForm wreckExportForm)
    {
        AutoMap(CultureInfo.InvariantCulture);
        if ( !wreckExportForm.Created )
            Map(w => w.Created).Ignore();
        if ( !wreckExportForm.LastModified )
            Map(w => w.LastModified).Ignore();
        if ( !wreckExportForm.Name )
            Map(w => w.Name).Ignore();
        if ( !wreckExportForm.Type )
            Map(w => w.Type).Ignore();
        if ( !wreckExportForm.ConstructionDetails )
            Map(w => w.ConstructionDetails).Ignore();
        if ( !wreckExportForm.YearConstructed )
            Map(w => w.YearConstructed).Ignore();
        if ( !wreckExportForm.DateOfLoss )
            Map(w => w.DateOfLoss).Ignore();
        if ( !wreckExportForm.InUkWaters )
            Map(w => w.InUkWaters).Ignore();
        if ( !wreckExportForm.IsWarWreck )
            Map(w => w.IsWarWreck).Ignore();
        if ( !wreckExportForm.IsAnAircraft )
            Map(w => w.IsAnAircraft).Ignore();
        if ( !wreckExportForm.Latitude )
            Map(w => w.Latitude).Ignore();
        if ( !wreckExportForm.Longitude )
            Map(w => w.Longitude).Ignore();
        if ( !wreckExportForm.IsProtectedSite )
            Map(w => w.IsProtectedSite).Ignore();
        if ( !wreckExportForm.ProtectionLegislation )
            Map(w => w.ProtectionLegislation).Ignore();
        if ( !wreckExportForm.AdditionalInformation )
            Map(w => w.AdditionalInformation).Ignore();
        if ( !wreckExportForm.OwnerName )
            Map(w => w.OwnerName).Ignore();
        if ( !wreckExportForm.OwnerEmail )
            Map(w => w.OwnerEmail).Ignore();
        if ( !wreckExportForm.OwnerNumber )
            Map(w => w.OwnerNumber).Ignore();
        if ( !wreckExportForm.OwnerAddress )
            Map(w => w.OwnerAddress).Ignore();
    }

}