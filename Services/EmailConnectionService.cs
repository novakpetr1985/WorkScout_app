using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace WorkScout.Services
{
    public sealed record EmailConnectionResult(bool Success, string Message);

    /// <summary>
    /// FEATURE: GMAIL SETUP
    /// Ověřuje samostatný Gmail účet přes IMAP/SMTP a odesílá úvodní kontrolní zprávu.
    /// </summary>
    /// <remarks>
    /// EXTENSION POINT: EMAIL PROVIDERS
    /// Verze 1.0.0 podporuje výhradně Gmail. Před podporou jiného poskytovatele
    /// přesuň hostitele a porty do typované konfigurace a odděl provider adaptér.
    /// </remarks>
    public sealed class EmailConnectionService
    {
        private const string ImapHost = "imap.gmail.com";
        private const int ImapPort = 993;
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;

        public async Task<EmailConnectionResult> VerifyConnectionAsync(string email, string appPassword)
        {
            try
            {
                using var imap = new ImapClient();
                await imap.ConnectAsync(ImapHost, ImapPort, SecureSocketOptions.SslOnConnect);
                await imap.AuthenticateAsync(email, appPassword);
                await imap.DisconnectAsync(true);

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(SmtpHost, SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(email, appPassword);
                await smtp.DisconnectAsync(true);

                return new EmailConnectionResult(true, "Připojení ověřeno.");
            }
            catch (AuthenticationException)
            {
                return new EmailConnectionResult(
                    false,
                    "Přihlášení se nezdařilo - zkontroluj e-mail a app password (ne běžné heslo k účtu).");
            }
            catch (Exception ex)
            {
                return new EmailConnectionResult(false, $"Chyba připojení: {ex.Message}");
            }
        }

        public async Task SendTestEmailAsync(string fromEmail, string appPassword, string toEmail)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(fromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "WorkScout - testovací e-mail";
            message.Body = new TextPart("plain")
            {
                Text = "Toto je automaticky odeslaný testovací e-mail z aplikace WorkScout. " +
                       "Pokud dorazil, propojení aplikačního a hlavního e-mailu funguje."
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(SmtpHost, SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(fromEmail, appPassword);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
