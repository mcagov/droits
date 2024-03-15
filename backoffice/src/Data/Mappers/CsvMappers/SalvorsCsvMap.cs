using System.Collections;
using System.Globalization;
using AutoMapper;
using CsvHelper.Configuration;
using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;
using Droits.Models.FormModels;

namespace Droits.Data.Mappers.CsvMappers;

public sealed class SalvorsCsvMap : ClassMap<SalvorExportDto>
{
    public SalvorsCsvMap(SalvorExportForm salvorExportForm)
    {
        AutoMap(CultureInfo.InvariantCulture);
        if ( !salvorExportForm.Created )
            Map(d => d.Created).Ignore();
        if ( !salvorExportForm.LastModified )
            Map(d => d.LastModified).Ignore();
        if ( !salvorExportForm.Name )
            Map(d => d.Name).Ignore();
        if ( !salvorExportForm.Email )
            Map(d => d.Email).Ignore();
        if ( !salvorExportForm.TelephoneNumber )
            Map(d => d.TelephoneNumber).Ignore();
        if ( !salvorExportForm.MobileNumber )
            Map(d => d.MobileNumber).Ignore();
        if ( !salvorExportForm.AddressPostcode )
            Map(d => d.AddressPostcode).Ignore();
        if ( !salvorExportForm.AddressCounty )
            Map(d => d.AddressCounty).Ignore();
        if ( !salvorExportForm.AddressTown )
            Map(d => d.AddressTown).Ignore();
        if ( !salvorExportForm.AddressLine1 )
            Map(d => d.AddressLine1).Ignore();
        if ( !salvorExportForm.AddressLine2 )
            Map(d => d.AddressLine2).Ignore();
    }

}