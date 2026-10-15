#region

using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Droits.Data.Mappers.CsvMappers;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace Droits.Helpers;

public static class ExportHelper
{
    public static async Task<byte[]> ExportRecordsAsync<T>( List<T> records, ClassMap<T>? classMap)
    {
        if (records.IsNullOrEmpty())
        {
            throw new Exception("No Records to export");
        }


        var csvData = new StringWriter();
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        var csv = new CsvWriter(csvData, csvConfiguration);

        if ( classMap != null )
        {
            csv.Context.RegisterClassMap(classMap);
        }

        await csv.WriteRecordsAsync(records);
        var fileContents = Encoding.UTF8.GetBytes(csvData.ToString());

        return fileContents;
    }


    public static async Task WriteRecordsAsync<T>(IAsyncEnumerable<T> records, ClassMap<T>? classMap,
        Stream output, CancellationToken cancellationToken = default)
    {
        await using var writer = new StreamWriter(output, new UTF8Encoding(false), 64 * 1024, leaveOpen: true);
        await using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

        if ( classMap != null )
        {
            csv.Context.RegisterClassMap(classMap);
        }

        await csv.WriteRecordsAsync(records, cancellationToken);
    }
}
