using System.Security.Cryptography;
using System.Text;

namespace WorkScout.Services
{
    /// <summary>
    /// SECURITY: Chrání Gmail app password pomocí Windows DPAPI pro aktuálního uživatele.
    /// Přenesená databáze proto sama o sobě nestačí k dešifrování credentials pod jiným účtem.
    /// </summary>
    public static class SecureStorageService
    {
        public static string Encrypt(string plainText)
        {
            var bytes = Encoding.UTF8.GetBytes(plainText);
            var encrypted = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string encryptedBase64)
        {
            var bytes = Convert.FromBase64String(encryptedBase64);
            var decrypted = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
