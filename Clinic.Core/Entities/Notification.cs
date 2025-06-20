namespace Clinic.Core.Entities
{
    public class Notification : BaseEntity<int>
    {
        #region Properties
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAT { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationStatus STAT { get; set; } = NotificationStatus.Pending;
        #endregion

        #region ForeignKeys
        public int? VetId { get; set; }
        public int?PatientId { get; set; }
        #endregion

        #region Relationships
        public Vet Vet { get; set; }
        public Patient Patient { get; set; }
        #endregion
    }
}