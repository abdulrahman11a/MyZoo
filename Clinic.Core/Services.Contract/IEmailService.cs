namespace Clinic.Core.Services.Contract
{
    public interface IEmailService
    {
        // Sends a single email with attachments
        Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, List<Attachment> attachments, string imagePath);

        // Sends email to multiple recipients
        Task SendEmailToMultipleRecipientsAsync();

          Task SendEmailAsync(string toEmail, string subject, string body, string? imagePath = null);
    }
}
