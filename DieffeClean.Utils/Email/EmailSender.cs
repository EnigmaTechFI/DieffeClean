using System.Net.Mail;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace DieffeClean.Utils.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(Message message, string templatePATH, List<(string, string)> replacer)
        {
            var mailMessage = CreateEmailMessage(message, templatePATH, replacer);
            await SendAsync(mailMessage);
        }
        
        private MimeMessage CreateEmailMessage(Message message, string templatePATH, List<(string, string)> replacer)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("noreply | Swirkey", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            if (!string.IsNullOrEmpty(templatePATH))
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {Text = EmailGetBody(templatePATH, replacer)};
            else
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) {Text = message.Content};
            return emailMessage;
        }

        private string EmailGetBody(string templatePATH, List<(string, string)> replacer)
        {
            
            StreamReader str = new(templatePATH);
            string MailText = str.ReadToEnd();
            str.Close();
            foreach (var item in replacer)
                MailText = MailText.Replace(item.Item1, item.Item2);
            return MailText;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(mailMessage);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
