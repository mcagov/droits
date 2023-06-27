namespace Droits.Models.Entities;
using Droits.Models;

public static class DatabaseSeeder
{
    public static void SeedData(DroitsContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        if (!dbContext.Wrecks.Any())
        {
            dbContext.Wrecks.AddRange(GetWrecks());
        }

        dbContext.SaveChanges();
    }

    private static List<Wreck> GetWrecks() => new List<Wreck>{
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "Titanic",
            Latitude = "41.7325",
            Longitude = "49.9469",
            DateOfLoss = new DateTime(1912, 4, 15),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "Bismarck",
            Latitude = "48.1667",
            Longitude = "-16.8667",
            DateOfLoss = new DateTime(1941, 5, 27),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "USS Arizona",
            Latitude = "21.3646",
            Longitude = "-157.9492",
            DateOfLoss = new DateTime(1941, 12, 7),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "HMS Victory",
            Latitude = "50.7997",
            Longitude = "-1.1095",
            DateOfLoss = new DateTime(1744, 10, 5),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "Andrea Doria",
            Latitude = "40.4497",
            Longitude = "-69.6814",
            DateOfLoss = new DateTime(1956, 7, 25),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "Birkenhead",
            Latitude = "-34.5966",
            Longitude = "19.3605",
            DateOfLoss = new DateTime(1852, 2, 26),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "Vasa",
            Latitude = "59.3293",
            Longitude = "18.0721",
            DateOfLoss = new DateTime(1628, 8, 10),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        }
    };

}
