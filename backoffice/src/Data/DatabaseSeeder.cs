using System.Runtime.InteropServices.JavaScript;
using Droits.Models.Entities;
using Bogus;
using Droits.Models.Enums;

namespace Droits.Data;

public static class DatabaseSeeder
{
    private static Faker _faker;
    public static void SeedData(DroitsContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        _faker = new Faker("en_GB");

        if (!dbContext.Wrecks.Any())
        {
            dbContext.Wrecks.AddRange(GetWrecks());
            dbContext.SaveChanges();
        }

        if (!dbContext.Salvors.Any())
        {
            dbContext.Salvors.AddRange(GetSalvors());
            dbContext.SaveChanges();
        }
        
        if (!dbContext.Droits.Any())
        {
            dbContext.Droits.AddRange(GetDroits(dbContext.Wrecks.ToList()));
            dbContext.SaveChanges();
        }

    }

    private static List<Salvor> GetSalvors() => Enumerable.Range(0, 5)
        .Select(i => new Salvor
        {
            Id = Guid.NewGuid(),
            Email = _faker.Internet.Email(),
            Name = _faker.Name.FullName(),
            TelephoneNumber = _faker.Phone.PhoneNumber(),
            Address = new Address
            {
                Line1 = _faker.Address.StreetAddress(),
                Line2 = _faker.Address.SecondaryAddress(),
                Town = _faker.Address.City(),
                County = _faker.Address.County(),
                Postcode = _faker.Address.ZipCode()
            },
            DateOfBirth = _faker.Date.Past(40, DateTime.UtcNow),
            Created = DateTime.Now,
            LastModified = DateTime.Now
        })
        .ToList();
    

    private static List<Droit> GetDroits(List<Wreck> wrecks) =>
        Enumerable.Range(0, 5)
            .Select(i => SeedDroit(_faker.Random.ArrayElement(wrecks.ToArray())))
            .ToList();


    private static Droit SeedDroit(Wreck wreck)
    {
        var reportedDate = _faker.Date.Past(3, DateTime.UtcNow);

        return new Droit
        {
            Id = Guid.NewGuid(),
            Reference = $"{_faker.Random.Int(0, 999).ToString("000")}/" +
                        $"{reportedDate.ToString("yy")}",
            Status = Enum.GetValues(typeof(DroitStatus))
                .OfType<DroitStatus>()
                .MinBy(x => Guid.NewGuid()),
            ReportedDate = reportedDate,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,

            WreckId = wreck.Id,

            WreckVesselName = wreck.Name,
            WreckVesselYearConstructed = _faker.Random.Int(1500, reportedDate.Year),
            WreckVesselYearSunk = _faker.Random.Int(1500, reportedDate.Year),

            Latitude = wreck.Latitude,
            Longitude = wreck.Longitude,
            LocationRadius = _faker.Random.Int(1, 500),
            Depth = _faker.Random.Int(1, 5000),

            ServicesDescription = _faker.Lorem.Sentence(),
            ServicesDuration = _faker.Random.Int(1, 24),
            ServicesEstimatedCost = _faker.Random.Int(1, 5000),

            District = _faker.Address.County(),
            LegacyFileReference = "",
            GoodsDischargedBy = _faker.Name.FullName(),
            DateDelivered = _faker.Date.Between(reportedDate, DateTime.UtcNow).ToShortDateString(),
            Agent = _faker.Name.FullName(),
            RecoveredFrom = _faker.Random.ArrayElement(new[] { "Afloat", "Ashore", "Seabed" }),
        };
    }

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
