namespace Clinic.APIS.DTOS.Notification_Dto
{
    public class RecentNotificationDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationStatus Status { get; set; }

        public string Vet { get; set; }
        public string Patient { get; set; }
    }
}
