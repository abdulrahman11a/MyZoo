namespace Clinic.Applacation
{
    public class EmailService : IEmailService
    {
        private readonly MailSetting _mailSetting;
        private readonly IUnitOfWork _unitOfWork;

        public EmailService(IOptions<MailSetting> mailSetting, IUnitOfWork unitOfWork)
        {
            _mailSetting = mailSetting.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, List<Attachment> attachments, string imagePath = "G:\\Advanced c#\\project\\Clinic_Mangement\\Clinic.APIS\\IMag\\WhatsApp Image 2025-05-05 at 21.15.52_243be64e.jpg")
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                return;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_mailSetting.Sender, _mailSetting.DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    mailMessage.Attachments.Add(attachment);
                }
            }

            using var smtpClient = new SmtpClient(_mailSetting.Host, _mailSetting.Port)
            {
                Credentials = new NetworkCredential(_mailSetting.Sender, _mailSetting.Password),
                EnableSsl = true
            };

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email to {toEmail}: {ex.Message}");
            }
        }

        public async Task SendEmailToMultipleRecipientsAsync()
        {   
            var spec = new GetAllAppointment();
            var upcomingAppointments = await _unitOfWork.Repository<Appointment, int>()
                .GetAllWithSpecAsync(spec);

            foreach (var appointment in upcomingAppointments)
            {
                var patientEmail = appointment.Patient?.Email;
                var vetEmail = appointment.Vet?.email;

                await SendReminderEmail(patientEmail, appointment);
                await SendReminderEmail(vetEmail, appointment);
            }
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, string imagePath= "G:\\Advanced c#\\project\\Clinic_Mangement\\Clinic.APIS\\IMag\\WhatsApp Image 2025-05-05 at 21.15.52_243be64e.jpg")
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                return;

            var message = new MailMessage
            {
                From = new MailAddress(_mailSetting.Sender, _mailSetting.DisplayName),
                Subject = subject,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            string htmlBody = $@"
            <html>
                <body>
                    <h1>{subject}</h1>
                    <p>{body}</p>
                    <img src='cid:welcomeImage' alt='Welcome Image'/>
                </body>
            </html>";

            var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

            if (!string.IsNullOrWhiteSpace(imagePath) && File.Exists(imagePath))
            {
                var img = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "welcomeImage",
                    TransferEncoding = TransferEncoding.Base64
                };
                htmlView.LinkedResources.Add(img);
            }


            message.AlternateViews.Add(htmlView);

            using var smtpClient = new SmtpClient(_mailSetting.Host, _mailSetting.Port)
            {
                Credentials = new NetworkCredential(_mailSetting.Sender, _mailSetting.Password),
                EnableSsl = true
            };
            try
            {
                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email to {toEmail}: {ex.Message}");
            }

        }

        private async Task SendReminderEmail(string toEmail, Appointment appointment, string imagePath = "G:\\Advanced c#\\project\\Clinic_Mangement\\Clinic.APIS\\IMag\\WhatsApp Image 2025-05-05 at 21.15.52_243be64e.jpg")
        {
            string subject = "Reminder: Upcoming Appointment";

            string body = $@"
    Dear,<br/>
    This is a reminder for your appointment scheduled at <b>{appointment.AppointmentDate}</b><br/>
    Purpose: {appointment.Purpose}<br/>
    Location: {appointment.Location?.Name}<br/><br/>
    <img src='cid:reminderImage' alt='Reminder Image' style='width:300px;'/><br/>
    Thank you.";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_mailSetting.Sender, _mailSetting.DisplayName),
                Subject = subject,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            // Create HTML view with image embedded
            var htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

            if (File.Exists(imagePath))
            {
                var linkedImage = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "reminderImage",
                    TransferEncoding = TransferEncoding.Base64,
                    ContentType = new ContentType("image/jpeg")
                };

                htmlView.LinkedResources.Add(linkedImage);
            }

            mailMessage.AlternateViews.Add(htmlView);

            using var smtpClient = new SmtpClient(_mailSetting.Host, _mailSetting.Port)
            {
                Credentials = new NetworkCredential(_mailSetting.Sender, _mailSetting.Password),
                EnableSsl = true
            };

            try
            {
                await smtpClient.SendMailAsync(mailMessage);

                var notification = new Notification
                {
                    Title = "Upcoming Appointment Reminder",
                    Message = $"Reminder for appointment on {appointment.AppointmentDate}",
                    CreatedAT = DateTime.Now,
                    NotificationType = NotificationType.AppointmentReminder,
                    VetId = appointment.VetId,
                    PatientId = appointment.PatientId,
                    STAT = NotificationStatus.Sent
                };

                await _unitOfWork.Repository<Notification, int>().AddAsync(notification);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");

                var failedNotification = new Notification
                {
                    Title = "Failed to Send Appointment Reminder",
                    Message = $"Failed to send reminder for appointment on {appointment.AppointmentDate}. Reason: {ex.Message}",
                    CreatedAT = DateTime.Now,
                    NotificationType = NotificationType.AppointmentReminder,
                    VetId = appointment.VetId,
                    PatientId = appointment.PatientId,
                    STAT = NotificationStatus.FailedToSend
                };

                await _unitOfWork.Repository<Notification, int>().AddAsync(failedNotification);
                await _unitOfWork.CompleteAsync();
            }
        }



    }
}
