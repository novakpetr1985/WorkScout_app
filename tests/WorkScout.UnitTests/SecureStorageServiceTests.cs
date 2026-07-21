using JobSearchApp.Services;

namespace WorkScout.UnitTests;

[TestFixture]
public class SecureStorageServiceTests
{
    [Test]
    public void EncryptAndDecrypt_ReturnsOriginalValueWithoutStoringPlainText()
    {
        const string plainText = "example-app-password";

        var encrypted = SecureStorageService.Encrypt(plainText);
        var decrypted = SecureStorageService.Decrypt(encrypted);

        Assert.Multiple(() =>
        {
            Assert.That(encrypted, Is.Not.EqualTo(plainText));
            Assert.That(decrypted, Is.EqualTo(plainText));
        });
    }
}
