namespace Clinic.APIS.DTOS.Notification_Dto
{
    public class NotificationDto
    {
        #region Properties
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAT { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationStatus STAT { get; set; }
        #endregion


        #region Relationships
        public string Vet { get; set; }
        public string Patient { get; set; }
        #endregion
    }
}