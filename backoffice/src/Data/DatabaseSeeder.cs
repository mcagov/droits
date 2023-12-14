
#region

using Bogus;
using Droits.Models.DTOs;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Newtonsoft.Json;

#endregion

namespace Droits.Data;

public static class DatabaseSeeder
{
    private static readonly Faker Faker;


    static DatabaseSeeder()
    {
        Faker = new Faker("en_GB");
    }


    public static void SeedData(DroitsContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        if ( !dbContext.Users.Any() )
        {
            dbContext.Users.AddRange(GetUsers());
            dbContext.SaveChanges();
        }
        
        if ( false && !dbContext.Wrecks.Any() )
        {
            dbContext.Wrecks.AddRange(GetWrecks(Faker.Random.ArrayElement(dbContext.Users.ToArray())));
            dbContext.SaveChanges();
        }

        if ( !dbContext.Salvors.Any() )
        {
            dbContext.Salvors.AddRange(GetSalvors(Faker.Random.ArrayElement(dbContext.Users.ToArray())));
            dbContext.SaveChanges();
        }
        
        if ( !dbContext.Droits.Any() )
        {
            dbContext.Droits.AddRange(GetDroits(dbContext.Wrecks,
                dbContext.Salvors, dbContext.Users));
            dbContext.SaveChanges();
        }

        if ( !dbContext.Letters.Any() )
        {
            dbContext.Letters.AddRange(GetLetters(dbContext.Droits, Faker.Random.ArrayElement(dbContext.Users.ToArray())));
            dbContext.SaveChanges();
        }

        if ( !dbContext.Notes.Any() )
        {
            SeedNotes(dbContext);
            dbContext.SaveChanges();
        }
        
        if ( !dbContext.Images.Any() )
        {
            dbContext.Images.AddRange(GetImages());
            dbContext.SaveChanges();
        }

    }


    private static IEnumerable<Letter> GetLetters(IEnumerable<Droit> droits, ApplicationUser user)
    {
        return Enumerable.Range(0, 15)
            .Select(i => new Letter
            {
                Id = new Guid(),
                DroitId = Faker.Random.ArrayElement(droits.ToArray()).Id,
                Recipient = Faker.Internet.Email(),
                Subject = $"{Faker.Vehicle.Manufacturer()} Subject",
                Body = Faker.Random.Words(300),
                Type = Enum.GetValues(typeof(LetterType))
                    .OfType<LetterType>()
                    .MinBy(x => Guid.NewGuid()),
                Status = Enum.GetValues(typeof(LetterStatus))
                    .OfType<LetterStatus>()
                    .Where(s => s != LetterStatus.Sent) 
                    .MinBy(x => Guid.NewGuid()),
                SenderUserId = new Guid(),
                Created = DateTime.Now,
                LastModified = DateTime.Now,
                LastModifiedByUserId = user.Id,
            })
            .ToList();
    }


    private static IEnumerable<Salvor> GetSalvors(ApplicationUser user)
    {
        return Enumerable.Range(0, 50)
            .Select(i => new Salvor
            {
                Id = Guid.NewGuid(),
                Email = Faker.Internet.Email(),
                Name = Faker.Name.FullName(),
                TelephoneNumber = Faker.Phone.PhoneNumber(),
                Address = new Address
                {
                    Line1 = Faker.Address.StreetAddress(),
                    Line2 = Faker.Address.SecondaryAddress(),
                    Town = Faker.Address.City(),
                    County = Faker.Address.County(),
                    Postcode = Faker.Address.ZipCode()
                },
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                LastModifiedByUserId = user.Id,
            })
            .ToList();
    }


    private static IEnumerable<Droit> GetDroits(IEnumerable<Wreck> wrecks, IEnumerable<Salvor> salvors, IEnumerable<ApplicationUser> users)
    {
        return Enumerable.Range(0, 50)
            .Select(i => SeedDroit(wrecks.Any()?Faker.Random.ArrayElement(wrecks.ToArray()):null,
                Faker.Random.ArrayElement(salvors.ToArray()),Faker.Random.ArrayElement(users.ToArray()) ))
            .ToList();
    }


    private static Droit SeedDroit(Wreck? wreck, Salvor salvor, ApplicationUser user)
    {
        var reportedDate = Faker.Date.Past(3, DateTime.UtcNow);

        return new Droit
        {
            Id = Guid.NewGuid(),
            AssignedToUserId = user.Id,
            Reference = $"{Faker.Random.Int(0, 99999):000}/" +
                        $"{reportedDate:yy}",
            Status = Enum.GetValues(typeof(DroitStatus))
                .OfType<DroitStatus>()
                .MinBy(x => Guid.NewGuid()),
            TriageNumber = Faker.Random.Int(1, 5).OrNull(Faker, 0.3f),
            ReportedDate = reportedDate,
            OriginalSubmission = GenerateOriginalSubmission(),
            DateFound = Faker.Date.Past(2, reportedDate),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            LastModifiedByUserId = user.Id,

            WreckId = wreck?.Id,
            IsHazardousFind = Faker.Random.Bool(),
            IsDredge = Faker.Random.Bool(),

            SalvorId = salvor.Id,

            Latitude = wreck?.Latitude,
            Longitude = wreck?.Longitude,
            InUkWaters = Faker.Random.Bool(),
            LocationRadius = Faker.Random.Int(1, 500),
            Depth = Faker.Random.Int(1, 5000),
            RecoveredFrom = Enum.GetValues(typeof(RecoveredFrom))
                .OfType<RecoveredFrom>()
                .MinBy(x => Guid.NewGuid()),
            LocationDescription = Faker.Lorem.Sentence(),

            SalvageAwardClaimed = Faker.Random.Bool(),
            ServicesDescription = Faker.Lorem.Sentence(),
            ServicesDuration = Faker.Lorem.Sentence(),
            ServicesEstimatedCost = Faker.Random.Int(1, 5000),
            MmoLicenceRequired = Faker.Random.Bool(),
            MmoLicenceProvided = Faker.Random.Bool(),
            SalvageClaimAwarded = Faker.Random.Float(),

            District = Faker.Address.County(),
            LegacyFileReference = Faker.Lorem.Sentence(),
            GoodsDischargedBy = Faker.Name.FullName(),
            DateDelivered = Faker.Date.Between(reportedDate, DateTime.UtcNow).ToShortDateString(),
            Agent = Faker.Name.FullName(),
            RecoveredFromLegacy = Faker.Random.ArrayElement(new[] { "Afloat", "Ashore", "Seabed" }),
            ImportedFromLegacy = Faker.Random.Bool(),
            LegacyRemarks = Faker.Lorem.Sentences()
        };
    }


    private static string GenerateOriginalSubmission()
    {
        var fakeData = new Faker<SubmittedReportDto>()
            .RuleFor(o => o.ReportDate, f => f.Date.Past(2).ToString("yyyy-MM-dd"))
            .RuleFor(o => o.WreckFindDate, f => f.Date.Past(3).ToString("yyyy-MM-dd"))
            .RuleFor(o => o.Latitude, f => f.Address.Latitude().OrNull(f, 0.3f))
            .RuleFor(o => o.Longitude, f => f.Address.Longitude().OrNull(f, 0.3f))
            .RuleFor(o => o.LocationRadius, f => f.Random.Number(1, 100).OrNull(f, 0.4f))
            .RuleFor(o => o.LocationDescription, f => f.Lorem.Sentence())
            .RuleFor(o => o.VesselName, f => f.Random.Word())
            .RuleFor(o => o.VesselConstructionYear, f => f.Date.Past(500).Year.ToString())
            .RuleFor(o => o.VesselSunkYear, f => f.Date.Past(20).Year.ToString())
            .RuleFor(o => o.VesselDepth, f => f.Random.Int(1, 100))
            .RuleFor(o => o.RemovedFrom, f => f.Random.ListItem(new[] { "Afloat", "Shipwreck", "Seabed", "Sea Shore" }))
            .RuleFor(o => o.WreckDescription, f => f.Lorem.Paragraph())
            .RuleFor(o => o.ClaimSalvage, f => f.PickRandom("yes", "no"))
            .RuleFor(o => o.SalvageServices, f => f.Random.Word())
            .RuleFor(o => o.Personal, f => new SubmittedPersonalDto()
            {
                FullName = f.Name.FullName(),
                Email = f.Internet.Email(),
                TelephoneNumber = f.Phone.PhoneNumber(),
                AddressLine1 = f.Address.StreetAddress(),
                AddressLine2 = f.Random.Word(),
                AddressTown = f.Address.City(),
                AddressCounty = f.Address.County(),
                AddressPostcode = f.Address.ZipCode()
            });
        
        var originalSubmission = fakeData.Generate();
        return JsonConvert.SerializeObject(originalSubmission, Formatting.Indented);

    }


    private static IEnumerable<Wreck> GetWrecks(ApplicationUser user)
    {
        return Enumerable.Range(0, 50)
            .Select(i => new Wreck
            {
                Id = Guid.NewGuid(),
                Name = Faker.Name.FullName(),
                ConstructionDetails = Faker.Lorem.Word(),
                YearConstructed = Faker.Random.Int(1500, DateTime.UtcNow.Year),
                DateOfLoss = Faker.Date.Past(500, DateTime.UtcNow),
                InUkWaters = Faker.Random.Bool(),
                IsWarWreck = Faker.Random.Bool(),
                IsAnAircraft = Faker.Random.Bool(),
                Latitude = Faker.Address.Latitude(),
                Longitude = Faker.Address.Longitude(),

                IsProtectedSite = Faker.Random.Bool(),
                ProtectionLegislation = Faker.Lorem.Word(),
                AdditionalInformation = Faker.Lorem.Sentence(),

                OwnerName = Faker.Name.FullName(),
                OwnerEmail = Faker.Internet.Email(),
                OwnerNumber = Faker.Phone.PhoneNumber(),

                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                LastModifiedByUserId = user.Id,
            })
            .ToList();
    }
    
    
    private static IEnumerable<ApplicationUser> GetUsers()
    {
        return Enumerable.Range(0, 5)
            .Select(i => new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                AuthId = Guid.NewGuid().ToString(),
                Email = Faker.Internet.Email(),
                Name = Faker.Name.FullName(),
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
            })
            .ToList();
    }
    
    private static void SeedNotes(DroitsContext dbContext)
    {
        var notes = new List<Note>();

        notes.AddRange(GenerateNotesForEntities(dbContext, dbContext.Droits, nameof(Note.DroitId)));
        notes.AddRange(GenerateNotesForEntities(dbContext, dbContext.Wrecks, nameof(Note.WreckId)));
        notes.AddRange(GenerateNotesForEntities(dbContext, dbContext.Salvors, nameof(Note.SalvorId)));
        notes.AddRange(GenerateNotesForEntities(dbContext, dbContext.Letters, nameof(Note.LetterId)));
    
        dbContext.Notes.AddRange(notes);
    }


    private static IEnumerable<Note> GenerateNotesForEntities<T>(DroitsContext dbContext,
        IEnumerable<T> entities, string propertyName) where T : BaseEntity
    {
        var notes = new List<Note>();
        var randomUser = Faker.Random.ArrayElement(dbContext.Users.ToArray());

        foreach ( var entity in entities )
        {
            var numberOfNotes = Faker.Random.Int(1, 5);
            for ( var i = 0; i < numberOfNotes; i++ )
            {
                var note = new Note
                {
                    Title = $"{Faker.Music.Genre()} Note",
                    Text = Faker.Lorem.Paragraphs(),
                    Type = Enum.GetValues(typeof(NoteType))
                        .OfType<NoteType>()
                        .MinBy(x => Guid.NewGuid()),
                    Created = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    LastModifiedByUserId = randomUser.Id,
                };

                typeof(Note).GetProperty(propertyName)?.SetValue(note, entity.Id);
                notes.Add(note);
            }
        }

        return notes;
    }


    private static IEnumerable<Image> GetImages()
    {
        return Enumerable.Range(0, 1)
            .Select(i => new Image()
            {
                Id = new Guid(),
                Key="test-image",
                FileContentType = "image/png",
                Filename = "MaritimeAgency.svg.png",
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
            })
            .ToList();
    }
}
