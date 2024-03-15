using System.Collections;
using System.Globalization;
using AutoMapper;
using CsvHelper.Configuration;
using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.FormModels.ExportFormModels;

namespace Droits.Data.Mappers.CsvMappers;

public sealed class LettersCsvMap : ClassMap<LetterExportDto>
{
    public LettersCsvMap(LetterExportForm letterExportForm)
    {
        AutoMap(CultureInfo.InvariantCulture);
        if ( !letterExportForm.Recipient )
            Map(l => l.Recipient).Ignore();
        if ( !letterExportForm.DroitReference )
            Map(l => l.DroitReference).Ignore();
        if ( !letterExportForm.QualityApprovedUser )
            Map(l => l.QualityApprovedUser).Ignore();
        if ( !letterExportForm.Status )
            Map(l => l.Status).Ignore();
        if ( !letterExportForm.Type )
            Map(l => l.Type).Ignore();
    }

}