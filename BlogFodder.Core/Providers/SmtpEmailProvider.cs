using System.Text;
using BlogFodder.Core.Plugins.Interfaces;
using BlogFodder.Core.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace BlogFodder.Core.Providers;

public class SmtpEmailProvider : IEmailProvider
    {
        private readonly BlogFodderSettings _gabSettings;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SmtpEmailProvider(IOptions<BlogFodderSettings> gabSettings, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _gabSettings = gabSettings.Value;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendEmailWithTemplateAsync(string toEmail, string subject, List<string> paragraphs)
        {
            // Get the default email template and the logo
            //string webRootPath = _env.WebRootPath;
            var emailTemplateUrl = $"{_env.WebRootPath}{Path.DirectorySeparatorChar}templates{Path.DirectorySeparatorChar}email{Path.DirectorySeparatorChar}{Constants.Assets.DefaultEmailTemplate}";
            var logoUrl = string.Empty; //_httpContextAccessor.ToAbsoluteUrl(_gabSettings.LogoPng);

            // Get template html
            using var sourceReader = File.OpenText(emailTemplateUrl);
            var emailTemplate = await sourceReader.ReadToEndAsync();

            // send email
            using var smtp = new SmtpClient();

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_gabSettings.Email.SenderEmail));
            emailMessage.To.Add(MailboxAddress.Parse(toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = HtmlBody(paragraphs, emailTemplate, logoUrl)
            };

            await smtp.ConnectAsync(_gabSettings.Email.Smtp.Host, _gabSettings.Email.Smtp.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_gabSettings.Email.Smtp.Username, _gabSettings.Email.Smtp.Password);
            await smtp.SendAsync(emailMessage).ConfigureAwait(false);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            // send email
            using var smtp = new SmtpClient();

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_gabSettings.Email.SenderEmail));
            emailMessage.To.Add(MailboxAddress.Parse(toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message
            };

            await smtp.ConnectAsync(_gabSettings.Email.Smtp.Host, _gabSettings.Email.Smtp.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_gabSettings.Email.Smtp.Username, _gabSettings.Email.Smtp.Password);
            await smtp.SendAsync(emailMessage).ConfigureAwait(false);
            await smtp.DisconnectAsync(true);
        }

        private static string FormatParagraphs(string text)
        {
            return $"<p style=\"Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:17px;font-family:helvetica, 'helvetica neue', arial, verdana, sans-serif;line-height:28px;color:#333333\">{text}</p>";
        }

        private static string HtmlBody(List<string> paragraphs, string emailTemplate, string logoUrl)
        {
            // Replace logo
            emailTemplate = emailTemplate.Replace("##LOGO##", logoUrl);

            var sb = new StringBuilder();
            foreach (var para in paragraphs)
            {
                sb.AppendLine(FormatParagraphs(para));
            }

            // Replace content
            emailTemplate = emailTemplate.Replace("##CONTENT##", sb.ToString());

            return emailTemplate;
        }
    }