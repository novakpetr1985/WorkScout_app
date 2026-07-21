using JobSearchApp.Services;

namespace WorkScout.UnitTests;

[TestFixture]
public class TestJobSourceTests
{
    [Test]
    public async Task LoadAsync_ReturnsValidDemonstrationListings()
    {
        var source = new TestJobSource();

        var listings = await source.LoadAsync();

        Assert.Multiple(() =>
        {
            Assert.That(source.Name, Is.EqualTo("Ukázková data"));
            Assert.That(listings, Has.Count.EqualTo(4));
            Assert.That(listings, Has.All.Property("Company").Not.Empty);
            Assert.That(listings, Has.All.Property("Position").Not.Empty);
            Assert.That(listings, Has.All.Property("Url").StartWith("https://"));
        });
    }

    [Test]
    public void LoadAsync_WithCancelledToken_ThrowsOperationCanceledException()
    {
        var source = new TestJobSource();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        Assert.That(
            async () => await source.LoadAsync(cancellation.Token),
            Throws.InstanceOf<OperationCanceledException>());
    }
}
