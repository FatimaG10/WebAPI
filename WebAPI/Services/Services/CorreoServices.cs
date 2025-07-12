using MailKit.Net.Smtp;
using MimeKit;

namespace WebAPI.Services.Services
{
    public class CorreoServices
    {
        private readonly IConfiguration _config;

        public CorreoServices(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnviarEmailAsync(string destinatario, string asunto, string htmlContent)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:User"]));
            email.To.Add(MailboxAddress.Parse(destinatario));
            email.Subject = asunto;

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlContent
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["EmailSettings:User"], _config["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
