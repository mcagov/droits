using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace Droits.Data;

public static class MigrationExtensions
{
    public static bool TableExists(this MigrationBuilder migrationBuilder, string tableName)
    {
        var sql = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
        
        // Execute the SQL query to check if the table exists
        var result = migrationBuilder.Sql(sql);
        
        // Since Sql method returns void, we assume the query was executed successfully
        // We parse the result to check if the count is greater than 0
        return result != null && int.TryParse(result.ToString(), out int count) && count > 0;
    }
}