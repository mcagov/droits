using System.Collections;
using System.Globalization;
using AutoMapper;
using CsvHelper.Configuration;
using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.FormModels.ExportFormModels;

namespace Droits.Data.Mappers.CsvMappers;

public sealed class SalvorsCsvMap : ClassMap<SalvorExportDto>
{
    public SalvorsCsvMap(SalvorExportForm salvorExportForm)
    {
        AutoMap(CultureInfo.InvariantCulture);
        if ( !salvorExportForm.Created )
            Map(s => s.Created).Ignore();
        if ( !salvorExportForm.LastModified )
            Map(s => s.LastModified).Ignore();
        if ( !salvorExportForm.Name )
            Map(s => s.Name).Ignore();
        if ( !salvorExportForm.Email )
            Map(s => s.Email).Ignore();
        if ( !salvorExportForm.TelephoneNumber )
            Map(s => s.TelephoneNumber).Ignore();
        if ( !salvorExportForm.MobileNumber )
            Map(s => s.MobileNumber).Ignore();
        if ( !salvorExportForm.AddressPostcode )
            Map(s => s.AddressPostcode).Ignore();
        if ( !salvorExportForm.AddressCounty )
            Map(s => s.AddressCounty).Ignore();
        if ( !salvorExportForm.AddressTown )
            Map(s => s.AddressTown).Ignore();
        if ( !salvorExportForm.AddressLine1 )
            Map(s => s.AddressLine1).Ignore();
        if ( !salvorExportForm.AddressLine2 )
            Map(s => s.AddressLine2).Ignore();
    }

}