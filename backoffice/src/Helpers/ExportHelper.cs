#region

using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.IdentityModel.Tokens;
using Droits.Data.Mappers.CsvMappers;
using Droits.Models.FormModels;

#endregion

namespace Droits.Helpers;

public static class ExportHelper
{
    public static async Task<byte[]> ExportRecordsAsync<T>( List<T> records, ClassMap<T> classMap)
    {
        if (records.IsNullOrEmpty())
        {
            throw new Exception("No Records to export");
        }
    

        var csvData = new StringWriter();
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        var csv = new CsvWriter(csvData, csvConfiguration);

        // var classMap = new DroitsCsvMap(new ExportFieldsForm() { Id = true });
        
        csv.Context.RegisterClassMap(classMap);
    
        await csv.WriteRecordsAsync(records);
        var fileContents = Encoding.UTF8.GetBytes(csvData.ToString());
    
        return fileContents;
    }
}