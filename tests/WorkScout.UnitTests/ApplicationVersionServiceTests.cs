using JobSearchApp.Services;

namespace WorkScout.UnitTests;

[TestFixture]
public class ApplicationVersionServiceTests
{
    [Test]
    public void Current_ReturnsSupportedSemanticVersionFromApplicationAssembly()
    {
        Assert.Multiple(() =>
        {
            Assert.That(
                ApplicationVersionService.Current,
                Does.Match(@"^1\.0\.0(?:-(?:feature|dev|test|rc))?$"));
            Assert.That(
                ApplicationVersionService.DisplayName,
                Is.EqualTo($"WorkScout {ApplicationVersionService.Current}"));
        });
    }
}
