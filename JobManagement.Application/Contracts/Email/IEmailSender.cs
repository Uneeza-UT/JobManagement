using JobManagement.Application.Models.Email;

namespace JobManagement.Application.Contracts.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(EmailMessageData emailMessageData);
    }
}
