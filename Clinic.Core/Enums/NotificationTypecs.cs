namespace Clinic.Core.Enums
{
    public enum NotificationType
    {
        [EnumMember(Value = "AppointmentReminder")]
        AppointmentReminder,
        [EnumMember(Value = "TaskReminder")]
        TaskReminder,
        [EnumMember(Value = "FollowUp")]
        FollowUp,
        [EnumMember(Value = "SystemAlert")]
        SystemAlert,
        [EnumMember(Value = "GeneralNotice")]
        GeneralNotice       
    }

}
