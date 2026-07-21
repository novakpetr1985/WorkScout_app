using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace JobSearchApp.Services
{
    public class EmailConnectionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    // Zatím natvrdo na Gmail (imap.gmail.com / smtp.gmail.com) - projekt počítá
    // s tím, že "e-mail pro aplikaci" je vždy nově založený Gmail účet.
    // Pokud by se v budoucnu hodilo připojit i jiného poskytovatele, stačí
    // tyhle konstanty vytáhnout do konfigurace.
    public class EmailConnectionService
    {
        private const string ImapHost = "imap.gmail.com";
        private const int ImapPort = 993;
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;

        // Skutečně se přihlásí na IMAP i SMTP se zadanými údaji - ne jen
        // kontrola formátu e-mailu. Tohle je ta "ať si to sama ověří" logika.
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

                return new EmailConnectionResult { Success = true, Message = "Připojení ověřeno." };
            }
            catch (AuthenticationException)
            {
                return new EmailConnectionResult
                {
                    Success = false,
                    Message = "Přihlášení se nezdařilo - zkontroluj e-mail a app password (ne běžné heslo k účtu)."
                };
            }
            catch (Exception ex)
            {
                return new EmailConnectionResult { Success = false, Message = $"Chyba připojení: {ex.Message}" };
            }
        }

        // Pošle testovací e-mail z app účtu na uživatelův e-mail - použije se
        // hned po dokončení wizardu, ať má uživatel jistotu, že i tahle cesta funguje.
        public async Task SendTestEmailAsync(string fromEmail, string appPassword, string toEmail)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(fromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "JobSearchApp - testovací e-mail";
            message.Body = new TextPart("plain")
            {
                Text = "Tohle je testovací e-mail z aplikace JobSearchApp. " +
                       "Pokud ti přišel, propojení mezi app e-mailem a tvým e-mailem funguje."
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(SmtpHost, SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(fromEmail, appPassword);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
