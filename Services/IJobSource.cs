using WorkScout.Models;

namespace WorkScout.Services
{
    /// <summary>
    /// EXTENSION POINT: JOB SOURCES
    /// Jednotný kontrakt pro demo data, veřejná API, feedy i portálové adaptéry.
    /// Implementace nesmí aktualizovat UI ani ukládat credentials; vrací pouze
    /// normalizované nabídky a respektuje zrušení operace.
    /// </summary>
    public interface IJobSource
    {
        string Name { get; }

        Task<IReadOnlyList<JobListing>> LoadAsync(CancellationToken cancellationToken = default);
    }
}
