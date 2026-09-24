using JobManagement.Application.Contracts.Email;
using JobManagement.Application.Logging;
using JobManagement.Application.Models.Email;
using JobManagement.Infrastructure.Logging;
using Microsoft.Extensions.Options;
using Resend;

namespace JobManagement.Infrastructure.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly IResend _resend;
        public EmailSettings _emailSettings { get; }
        public IAppLogger<EmailSender> _appLogger { get; }


        public EmailSender(IResend resend, IOptions<EmailSettings> emailSettings, IAppLogger<EmailSender> appLogger)
        {
            _resend = resend;
            _emailSettings = emailSettings.Value;
            _appLogger = appLogger;
        }

        public async Task<bool> SendEmail(EmailMessageData emailMessageData)
        {
            try
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

            catch (Exception ex)
            {
                _appLogger.LogError(ex.Message);
                return false;
            }
        }
    }

}
