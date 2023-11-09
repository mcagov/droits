using Droits.Data;
using Microsoft.EntityFrameworkCore;

namespace Droits.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static DroitsContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<DroitsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            return new DroitsContext(options);
            
        }
    }
}