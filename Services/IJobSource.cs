using JobSearchApp.Models;

namespace JobSearchApp.Services
{
    public interface IJobSource
    {
        string Name { get; }

        Task<IReadOnlyList<JobListing>> LoadAsync(CancellationToken cancellationToken = default);
    }
}
