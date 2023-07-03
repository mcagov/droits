using Droits.Models.Entities;
using Bogus;
namespace Droits.Data;

public static class DatabaseSeeder
{
    public static void SeedData(DroitsContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        var faker = new Faker();

        if (!dbContext.Wrecks.Any())
        {
            dbContext.Wrecks.AddRange(GetWrecks());
        }

        if (!dbContext.Salvors.Any())
        {
            dbContext.Salvors.AddRange(GetSalvors(faker));
        }

        dbContext.SaveChanges();
    }

    private static List<Salvor> GetSalvors(Faker faker) => Enumerable.Range(0, 5)
        .Select(i => new Salvor
        {
            Id = Guid.NewGuid(),
            Email = faker.Internet.Email(),
            Name = faker.Name.FullName(),
            TelephoneNumber = faker.Phone.PhoneNumber(),
            Address = new Address
            {
                Line1 = faker.Address.StreetAddress(),
                Line2 = faker.Address.SecondaryAddress(),
                Town = faker.Address.City(),
                County = faker.Address.County(),
                Postcode = faker.Address.ZipCode(),
            },
            DateOfBirth = faker.Date.Past(40, DateTime.UtcNow),
            Created = DateTime.Now,
            LastModified = DateTime.Now
        })
        .ToList();

    private static List<Wreck> GetWrecks() => new List<Wreck>{
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "Titanic",
            Latitude = "41.726931",
            Longitude = "-49.948253",
            DateOfLoss = new DateTime(1912, 4, 15),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "Bismarck",
            Latitude = "48.67375",
            Longitude = "-16.214167",
            DateOfLoss = new DateTime(1941, 5, 27),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "Lusitania",
            Latitude = "51.396111",
            Longitude = "-8.4525",
            DateOfLoss = new DateTime(1915, 5, 7),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "Bounty",
            Latitude = "27.773889",
            Longitude = "-78.322778",
            DateOfLoss = new DateTime(2012, 10, 29),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "Mary Rose",
            Latitude = "50.794444",
            Longitude = "-1.108333",
            DateOfLoss = new DateTime(1545, 7, 19),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "Vasa",
            Latitude = "59.328056",
            Longitude = "18.091111",
            DateOfLoss = new DateTime(1628, 8, 10),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "HMS Hood",
            Latitude = "63.03",
            Longitude = "-31.16",
            DateOfLoss = new DateTime(1941, 5, 24),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "HMS Victory",
            Latitude = "50.7653",
            Longitude = "-1.2048",
            DateOfLoss = null,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "USS Arizona",
            Latitude = "21.365",
            Longitude = "-157.9497",
            DateOfLoss = new DateTime(1941, 12, 7),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "Andrea Doria",
            Latitude = "40.7427",
            Longitude = "-69.8436",
            DateOfLoss = new DateTime(1956, 7, 25),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "SS Edmund Fitzgerald",
            Latitude = "46.9950",
            Longitude = "-85.1496",
            DateOfLoss = new DateTime(1975, 11, 10),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        },
        new Wreck
        {
            Id = Guid.NewGuid(),
            Name = "USS Monitor",
            Latitude = "36.95",
            Longitude = "-75.94",
            DateOfLoss = null,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        }
    };

}
