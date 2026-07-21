using System.Security.Cryptography;
using System.Text;

namespace JobSearchApp.Services
{
    // Zašifruje/dešifruje app password pomocí Windows DPAPI.
    // Klíč je vázaný na přihlášeného Windows uživatele - nikdo jiný, kdo by
    // zkopíroval soubor jobsearch.db, ho bez tvého Windows účtu nerozšifruje.
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
