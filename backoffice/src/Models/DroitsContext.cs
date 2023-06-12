using Microsoft.EntityFrameworkCore;

namespace Droits.Models;

public partial class DroitsContext : DbContext
{
    public DroitsContext()
    {
    }

    public DroitsContext(DbContextOptions<DroitsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Droit> Droits { get; set; } = null!;
    public virtual DbSet<Email> Emails { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Droit>(entity =>
        {
            entity.ToTable("droits");

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Id);
            entity.Property(e => e.Status);
            entity.Property(e => e.ReportedDate);
            entity.Property(e => e.Created);
            entity.Property(e => e.Modified);
            entity.Property(e => e.Reference);
            entity.Property(e => e.IsHazardousFind);

            //Wreck
            entity.Property(e => e.WreckConstructionDetails);
            entity.Property(e => e.WreckVesselName);
            entity.Property(e => e.WreckVesselYearConstructed);
            entity.Property(e => e.WreckVesselYearSunk);

            // Location
            entity.Property(e => e.Latitude);
            entity.Property(e => e.Longitude);
            entity.Property(e => e.InUkWaters);
            entity.Property(e => e.LocationRadius);
            entity.Property(e => e.Depth);
            entity.Property(e => e.LocationDescription);

            // Salvage

            entity.Property(e => e.SalvageAwardClaimed);
            entity.Property(e => e.ServicesDescription);
            entity.Property(e => e.ServicesDuration);
            entity.Property(e => e.ServicesEstimatedCost);
            entity.Property(e => e.MMOLicenceRequired);
            entity.Property(e => e.MMOLicenceProvided);
            entity.Property(e => e.SalvageClaimAwarded);

            // Legacy fields

            entity.Property(e => e.District);
            entity.Property(e => e.LegacyFileReference);
            entity.Property(e => e.GoodsDischargedBy);
            entity.Property(e => e.DateDelivered);
            entity.Property(e => e.Agent);
            entity.Property(e => e.RecoveredFrom);
            entity.Property(e => e.ImportedFromLegacy);
        });
        
        modelBuilder.Entity<Email>(entity =>
        {
            entity.ToTable("emails");

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Id);
            entity.Property(e => e.Subject);
            entity.Property(e => e.Body);
            entity.Property(e => e.DateSent);
            entity.Property(e => e.SenderEmailAddress);
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}