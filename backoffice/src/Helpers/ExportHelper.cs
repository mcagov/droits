#region

using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace Droits.Helpers;

public static class ExportHelper
{
    public static async Task<byte[]> ExportRecordsAsync<T>( List<T> records)
    {
        if (records.IsNullOrEmpty())
        {
            throw new Exception("No Records to export");
        }
    

        var csvData = new StringWriter();
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        var csv = new CsvWriter(csvData, csvConfiguration);
    
        await csv.WriteRecordsAsync(records);
        var fileContents = Encoding.UTF8.GetBytes(csvData.ToString());
    
        return fileContents;
    }
}