using JobSearchApp.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace JobSearchApp.Data
{
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

            // Výchozí seznam portálů - žádná zvláštní obrazovka na jejich správu
            // zatím není, ale jde je snadno přidat/upravit i ručně v DB.
            modelBuilder.Entity<Portal>().HasData(
                new Portal { Id = 1, Name = "Jobs.cz", IsSelected = true },
                new Portal { Id = 2, Name = "Prace.cz", IsSelected = true },
                new Portal { Id = 3, Name = "StartupJobs", IsSelected = true },
                new Portal { Id = 4, Name = "LinkedIn", IsSelected = false }
            );
        }
    }
}
