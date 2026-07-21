using WorkScout.Models;

namespace WorkScout.Services
{
    /// <summary>
    /// Poskytuje deterministická ukázková data pro první release bez síťových volání.
    /// </summary>
    /// <remarks>
    /// DEMO DATA: Až bude dostupný první produkční adaptér <see cref="IJobSource"/>,
    /// composition root zvolí skutečné zdroje. Tento zdroj může zůstat pouze pro
    /// ukázkový režim a automatické testy.
    /// </remarks>
    public sealed class DemoJobSource : IJobSource
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
