using JobSearchApp.Data;
using JobSearchApp.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace WorkScout.IntegrationTests;

[TestFixture]
public class DatabaseIntegrationTests
{
    private string _testDirectory = null!;
    private string _databasePath = null!;

    [SetUp]
    public void SetUp()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), $"WorkScout-tests-{Guid.NewGuid():N}");
        Directory.CreateDirectory(_testDirectory);
        _databasePath = Path.Combine(_testDirectory, "integration.db");
    }

    [TearDown]
    public void TearDown()
    {
        SqliteConnection.ClearAllPools();

        if (Directory.Exists(_testDirectory))
            Directory.Delete(_testDirectory, recursive: true);
    }

    [Test]
    public async Task InitialMigration_CreatesDatabaseAndSeedsPortals()
    {
        await using var db = CreateDbContext();

        await db.Database.MigrateAsync();
        var portals = await db.Portals.OrderBy(portal => portal.Id).ToListAsync();
        var appliedMigrations = await db.Database.GetAppliedMigrationsAsync();

        Assert.Multiple(() =>
        {
            Assert.That(File.Exists(_databasePath), Is.True);
            Assert.That(appliedMigrations, Does.Contain("20260721192424_InitialCreate"));
            Assert.That(portals.Select(portal => portal.Name), Is.EqualTo(new[]
            {
                "Jobs.cz", "Prace.cz", "StartupJobs", "LinkedIn"
            }));
            Assert.That(portals.Count(portal => portal.IsSelected), Is.EqualTo(3));
        });
    }

    [Test]
    public async Task ApplicationAndInboundMessage_CanBePersistedAndLoaded()
    {
        await using (var db = CreateDbContext())
        {
            await db.Database.MigrateAsync();
            var listing = new JobListing
            {
                Company = "Test Company",
                Position = "Test Position",
                Profession = "Testing",
                Portal = "Test Portal",
                Url = "https://example.test/jobs/1",
                DateFound = DateTime.Today
            };
            var application = new JobApplication
            {
                JobListing = listing,
                Status = ApplicationStatus.Sent,
                SubmittedAtUtc = DateTime.UtcNow,
                CvPath = @"C:\Documents\cv.pdf"
            };
            application.InboundMessages.Add(new InboundMessage
            {
                ExternalMessageId = "message-1@example.test",
                FromAddress = "recruiter@example.test",
                Subject = "Pozvánka k pohovoru",
                BodyPreview = "Dobrý den…",
                ReceivedAtUtc = DateTime.UtcNow
            });

            db.JobApplications.Add(application);
            await db.SaveChangesAsync();
        }

        await using var verificationDb = CreateDbContext();
        var saved = await verificationDb.JobApplications
            .Include(application => application.JobListing)
            .Include(application => application.InboundMessages)
            .SingleAsync();

        Assert.Multiple(() =>
        {
            Assert.That(saved.Status, Is.EqualTo(ApplicationStatus.Sent));
            Assert.That(saved.JobListing.Company, Is.EqualTo("Test Company"));
            Assert.That(saved.InboundMessages, Has.Count.EqualTo(1));
            Assert.That(saved.InboundMessages.Single().Subject, Is.EqualTo("Pozvánka k pohovoru"));
        });
    }

    [Test]
    public async Task DuplicateListingUrl_IsRejectedByUniqueIndex()
    {
        await using var db = CreateDbContext();
        await db.Database.MigrateAsync();
        db.JobListings.AddRange(CreateListing(), CreateListing());

        Assert.That(async () => await db.SaveChangesAsync(), Throws.TypeOf<DbUpdateException>());
    }

    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={_databasePath}")
            .Options;

        return new AppDbContext(options);
    }

    private static JobListing CreateListing() => new()
    {
        Company = "Duplicate Test",
        Position = "Tester",
        Profession = "Testing",
        Portal = "Test Portal",
        Url = "https://example.test/jobs/duplicate",
        DateFound = DateTime.Today
    };
}
