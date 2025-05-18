using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MimeKit;
using MailKit.Net.Smtp;

namespace Service
{
    public class EmailService : IEmailService
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAcc;

        public EmailService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAcc)
        {
            _linkGenerator = linkGenerator;
            _httpContextAcc = httpContextAcc;
        }

        public string GenerateLink(string userId, string code)
        {
            var callBackUri = _linkGenerator.GetUriByAction(
                httpContext: _httpContextAcc.HttpContext,
                action: "ConfirmEmail",
                controller: "Authentication",
                values: new { userId, code });

            return callBackUri;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "kvusov@bk.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, true);
                await client.AuthenticateAsync("kvusov@bk.ru", "rbnQhSqdNKqsbtgcwNrj");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
