using Droits.Data;
using Droits.Tests.Helpers;

namespace Droits.Tests.UnitTests.Data;

public class DatabaseFunctionsUnitTests
{
    [Fact]
    public void EnsureCreated_DoesNothing_WhenDatabaseIsNotPostgres()
    {
        using var dbContext = TestDbContextFactory.CreateDbContext();

        var exception = Record.Exception(() => DatabaseFunctions.EnsureCreated(dbContext));

        Assert.Null(exception);
    }

    [Fact]
    public void EnsureSmallestLevenshteinDistanceSql_OnlyCreatesFunctionWhenMissing()
    {
        var sql = DatabaseFunctions.EnsureSmallestLevenshteinDistanceSql;

        Assert.Contains("to_regprocedure('get_smallest_levenshtein_distance(text, text)') IS NULL", sql);
        Assert.DoesNotContain("CREATE OR REPLACE", sql);
    }
}
