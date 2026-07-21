using WorkScout.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace WorkScout.Data
{
    /// <summary>
    /// FEATURE: LOCAL DATABASE — jediný EF Core kontext pro lokální SQLite databázi
    /// uživatele i izolované databáze integračních testů.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public static string DatabasePath
        {
            get
            {
                var appDataDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "WorkScout");

                Directory.CreateDirectory(appDataDirectory);
                return Path.Combine(appDataDirectory, "workscout.db");
            }
        }

        public DbSet<AppSettingsEntity> AppSettings => Set<AppSettingsEntity>();
        public DbSet<Portal> Portals => Set<Portal>();
        public DbSet<JobListing> JobListings => Set<JobListing>();
        public DbSet<JobApplication> JobApplications => Set<JobApplication>();
        public DbSet<InboundMessage> InboundMessages => Set<InboundMessage>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite($"Data Source={DatabasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FEATURE MAP: Schéma už nyní drží stabilní identitu nastavení, zdroje,
            // nabídky, žádosti a příchozí komunikaci. UI 1.0.0 používá jen první dvě
            // oblasti; zbytek je záměrný datový kontrakt pro navazující release.
            modelBuilder.Entity<AppSettingsEntity>()
                .Property(settings => settings.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Portal>()
                .HasIndex(portal => portal.Name)
                .IsUnique();

            modelBuilder.Entity<JobListing>()
                .HasIndex(listing => listing.Url)
                .IsUnique();

            modelBuilder.Entity<JobApplication>()
                .HasOne(application => application.JobListing)
                .WithMany()
                .HasForeignKey(application => application.JobListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InboundMessage>()
                .HasIndex(message => message.ExternalMessageId)
                .IsUnique();

            modelBuilder.Entity<InboundMessage>()
                .HasOne(message => message.JobApplication)
                .WithMany(application => application.InboundMessages)
                .HasForeignKey(message => message.JobApplicationId)
                .OnDelete(DeleteBehavior.SetNull);

            // FEATURE: PORTAL FILTER
            // Seed drží stabilní ID pro první instalaci. Budoucí správa portálů musí
            // používat novou migraci nebo synchronizační službu, ne ruční editaci DB.
            modelBuilder.Entity<Portal>().HasData(
                new Portal { Id = 1, Name = "Jobs.cz", IsSelected = true },
                new Portal { Id = 2, Name = "Prace.cz", IsSelected = true },
                new Portal { Id = 3, Name = "StartupJobs", IsSelected = true },
                new Portal { Id = 4, Name = "LinkedIn", IsSelected = false }
            );
        }
    }
}
