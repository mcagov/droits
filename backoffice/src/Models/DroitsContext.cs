using Microsoft.EntityFrameworkCore;
using Droits.Models;

namespace Droits.Models
{
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
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
