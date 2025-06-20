namespace Clinic.Core.Enums
{
    public enum NotificationStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Sent")]
        Sent,
        [EnumMember(Value = "Read")]
        Read,
       FailedToSend
    }
}
