using Microsoft.EntityFrameworkCore;

namespace OpenEcologyApp.Data
{
    public class EcologyDbContext : DbContext
    {
        public EcologyDbContext(DbContextOptions<EcologyDbContext> options)
            : base(options)
        {
        }

        public DbSet<GrainHarvest> GrainHarvests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GrainHarvest>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<GrainHarvest>()
                .Property(g => g.Region)
                .IsRequired();

            modelBuilder.Entity<GrainHarvest>()
                .Property(g => g.Year)
                .IsRequired();

            modelBuilder.Entity<GrainHarvest>()
                .Property(g => g.Yield)
                .IsRequired();

            modelBuilder.Entity<GrainHarvest>()
                .Property(g => g.SownArea)
                .IsRequired();

            modelBuilder.Entity<GrainHarvest>()
                .Property(g => g.GrossHarvest)
                .IsRequired();
        }
    }
}
