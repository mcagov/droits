using Droits.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Droits.Data;

public partial class DroitsContext : DbContext
{
    public DroitsContext()
    {
    }

    public DroitsContext(DbContextOptions<DroitsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Droit> Droits { get; set; }
    public virtual DbSet<Wreck> Wrecks { get; set; }
    public virtual DbSet<Email> Emails { get; set; }
    public virtual DbSet<Salvor> Salvors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Droit>(entity =>
        {
            entity.ToTable("droits");

            entity.Property(d => d.Id);
            entity.Property(d => d.Status);
            entity.Property(d => d.ReportedDate);
            entity.Property(d => d.Created);
            entity.Property(d => d.LastModified);
            entity.Property(d => d.Reference);
            entity.Property(d => d.IsHazardousFind);

            //Wreck
            entity.Property(d => d.WreckConstructionDetails);
            entity.Property(d => d.WreckVesselName);
            entity.Property(d => d.WreckVesselYearConstructed);
            entity.Property(d => d.WreckVesselYearSunk);

            // Location
            entity.Property(d => d.Latitude);
            entity.Property(d => d.Longitude);
            entity.Property(d => d.InUkWaters);
            entity.Property(d => d.LocationRadius);
            entity.Property(d => d.Depth);
            entity.Property(d => d.LocationDescription);

            // Salvage

            entity.Property(d => d.SalvageAwardClaimed);
            entity.Property(d => d.ServicesDescription);
            entity.Property(d => d.ServicesDuration);
            entity.Property(d => d.ServicesEstimatedCost);
            entity.Property(d => d.MMOLicenceRequired);
            entity.Property(d => d.MMOLicenceProvided);
            entity.Property(d => d.SalvageClaimAwarded);

            // Legacy fields

            entity.Property(d => d.District);
            entity.Property(d => d.LegacyFileReference);
            entity.Property(d => d.GoodsDischargedBy);
            entity.Property(d => d.DateDelivered);
            entity.Property(d => d.Agent);
            entity.Property(d => d.RecoveredFrom);
            entity.Property(d => d.ImportedFromLegacy);
        });


        modelBuilder.Entity<Wreck>(entity =>
        {
            entity.ToTable("wrecks");

            entity.Property(w => w.Id);
            entity.Property(w => w.Status);
            entity.Property(w => w.Name);
            entity.Property(w => w.DateOfLoss);
            entity.Property(w => w.IsWarWreck);
            entity.Property(w => w.IsAnAircraft);
            entity.Property(w => w.Latitude);
            entity.Property(w => w.Longitude);
            entity.Property(w => w.IsProtectedSite);
            entity.Property(w => w.ProtectionLegislation);
            entity.Property(w => w.Created);
            entity.Property(w => w.LastModified);

        });

        modelBuilder.Entity<Email>(entity =>
        {
            entity.ToTable("emails");

            entity.Property(e => e.Id);
            entity.Property(e => e.Subject);
            entity.Property(e => e.Body);
            entity.Property(e => e.DateSent);
            entity.Property(e => e.Recipient);
            entity.Property(e => e.Created);
            entity.Property(e => e.LastModified);
            entity.Property(e => e.Type);
        });

        modelBuilder.Entity<Salvor>(entity =>
        {
            entity.ToTable("salvors");

            entity.Property(s => s.Id);
            entity.Property(s => s.Email);
            entity.Property(s => s.Name);
            entity.Property(s => s.TelephoneNumber);
            entity.Property(s => s.DateOfBirth);
            entity.Property(s => s.Created);
            entity.Property(s => s.LastModified);
            entity.OwnsOne(s => s.Address);

        });

        base.OnModelCreating(modelBuilder);

    }




    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
