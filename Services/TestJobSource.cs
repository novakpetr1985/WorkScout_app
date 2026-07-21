using JobSearchApp.Models;

namespace JobSearchApp.Services
{
    // TODO: DEVELOPMENT DATA
    // Po připojení prvního skutečného portálu tento zdroj odebrat z produkčního sestavení.
    public sealed class TestJobSource : IJobSource
    {
        public string Name => "Ukázková data";

        public Task<IReadOnlyList<JobListing>> LoadAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IReadOnlyList<JobListing> listings = new List<JobListing>
            {
                new()
                {
                    Company = "ElektroMont s.r.o.", Position = "Elektrikář - montáž",
                    Profession = "Elektrikář", Portal = "Jobs.cz",
                    Url = "https://example.com/1", DateFound = DateTime.Today
                },
                new()
                {
                    Company = "Softbit a.s.", Position = "Junior .NET vývojář",
                    Profession = "Programátor", Portal = "StartupJobs",
                    Url = "https://example.com/2", DateFound = DateTime.Today.AddDays(-1)
                },
                new()
                {
                    Company = "Logimex CZ", Position = "Skladník - směnný provoz",
                    Profession = "Skladník", Portal = "Prace.cz",
                    Url = "https://example.com/3", DateFound = DateTime.Today.AddDays(-1)
                },
                new()
                {
                    Company = "Elektro Cheb", Position = "Elektrikář - revize",
                    Profession = "Elektrikář", Portal = "Jobs.cz",
                    Url = "https://example.com/4", DateFound = DateTime.Today.AddDays(-2)
                }
            };

            return Task.FromResult(listings);
        }
    }
}
