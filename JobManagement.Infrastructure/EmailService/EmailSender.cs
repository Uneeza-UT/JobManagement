using JobManagement.Application.Contracts.Email;
using JobManagement.Application.Models.Email;
using Microsoft.Extensions.Options;
using Resend;

namespace JobManagement.Infrastructure.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly IResend _resend;
        public EmailSettings _emailSettings { get; }
        public EmailSender(IResend resend, IOptions<EmailSettings> emailSettings)
        {
            _resend = resend;
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmail(EmailMessageData emailMessageData)
        {
            var message = new EmailMessage()
            {
                From = $"{_emailSettings.FromName} <{_emailSettings.FromAddress}>",
                To = { emailMessageData.To },
                Subject = emailMessageData.Subject,
                TextBody = emailMessageData.Body
            };

            var response = await _resend.EmailSendAsync(message);
            return response.Success;
        }
    }

}
