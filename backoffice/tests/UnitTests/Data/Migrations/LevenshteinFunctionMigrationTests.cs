using Droits.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Droits.Tests.UnitTests.Data.Migrations;

public class LevenshteinFunctionMigrationTests
{
    [Fact]
    public void RestoreLevenshteinFunctionMigration_RecreatesSearchFunction()
    {
        var options = new DbContextOptionsBuilder<DroitsContext>()
            .UseNpgsql("Host=localhost;Database=droits")
            .Options;
        using var dbContext = new DroitsContext(options);
        var migrator = dbContext.GetService<IMigrator>();

        var migrationSql = migrator.GenerateScript(
            "20240415092217_AddedDroitClosedDate",
            "20261006120000_RestoreLevenshteinFunction");

        Assert.Contains(
            "CREATE OR REPLACE FUNCTION get_smallest_levenshtein_distance",
            migrationSql);
    }
}
