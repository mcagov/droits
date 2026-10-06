using Droits.Data;
using Droits.Helpers;
using Droits.Models.Entities;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

namespace Droits.Tests.UnitTests.Repositories;

public class DroitRepositoryUnitTests
{
    [Theory]
    [InlineData("Reference")]
    [InlineData("Salvor")]
    [InlineData("VerifiedWreck")]
    [InlineData("ReportedWreck")]
    [InlineData("TriageNumber")]
    [InlineData("Items")]
    [InlineData("RoW")]
    public async Task GetOrderedDroitsWithAssociations_OrdersDashboardColumnsInBothDirections(
        string orderColumn)
    {
        await using var dbContext = new DroitsContext(new DbContextOptionsBuilder<DroitsContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
        dbContext.Droits.AddRange(CreateDroit("A", new DateTime(2026, 4, 2), 1),
            CreateDroit("B", new DateTime(2026, 4, 1), 2));
        await dbContext.SaveChangesAsync();

        var repository = new DroitRepository(dbContext, Mock.Of<IAccountService>());
        var searchOptions = new SearchOptions { OrderColumn = orderColumn };
        var orderColumnExpression = ServiceHelper.GetOrderColumnExpression(searchOptions);

        var ascendingReferences = await repository
            .GetOrderedDroitsWithAssociations(orderColumnExpression, false)
            .Select(d => d.Reference)
            .ToListAsync();
        var descendingReferences = await repository
            .GetOrderedDroitsWithAssociations(orderColumnExpression, true)
            .Select(d => d.Reference)
            .ToListAsync();

        Assert.Equal(new[] { "A", "B" }, ascendingReferences);
        Assert.Equal(new[] { "B", "A" }, descendingReferences);
    }

    private static Droit CreateDroit(string suffix, DateTime reportedDate, int materialCount)
    {
        var droit = new Droit
        {
            Id = Guid.NewGuid(),
            Reference = suffix,
            ReportedDate = reportedDate,
            ReportedWreckName = suffix,
            TriageNumber = suffix == "A" ? 1 : 2,
            Salvor = new Salvor { Name = suffix },
            Wreck = new Wreck { Name = suffix },
            AssignedToUser = new ApplicationUser { Name = suffix }
        };

        for (var i = 0; i < materialCount; i++)
        {
            droit.WreckMaterials.Add(new WreckMaterial
            {
                Id = Guid.NewGuid(),
                DroitId = droit.Id,
                Droit = droit
            });
        }

        return droit;
    }
}
