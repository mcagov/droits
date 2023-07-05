using System.Runtime.InteropServices.JavaScript;
using Droits.Models.Entities;
using Bogus;
using Droits.Models.Enums;

namespace Droits.Data;

public static class DatabaseSeeder
{
    private static Faker _faker;

    static DatabaseSeeder(){
        _faker = new Faker("en_GB");
    }

    public static void SeedData(DroitsContext dbContext)
    {
        dbContext.Database.EnsureCreated();

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

    private static List<Wreck> GetWrecks() => Enumerable.Range(0, 5)
        .Select(i => new Wreck
        {
            Id = Guid.NewGuid(),
            Status = Enum.GetValues(typeof(WreckStatus))
                .OfType<WreckStatus>()
                .MinBy(x => Guid.NewGuid()),
            Name = _faker.Name.FullName(),
            DateOfLoss = _faker.Date.Past(500, DateTime.UtcNow),
            
            IsWarWreck = _faker.Random.Bool(),
            IsAnAircraft = _faker.Random.Bool(),
            Latitude = _faker.Address.Latitude().ToString(),
            Longitude = _faker.Address.Longitude().ToString(),
            
            IsProtectedSite = _faker.Random.Bool(),
            ProtectionLegislation = _faker.Lorem.Sentence(),
            
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        })
        .ToList();
}
