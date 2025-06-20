namespace Clinic.Core.Enums
{
    public enum AppointmentStatus
        {
        [EnumMember( Value ="Pending")]
        Pending,
        [EnumMember(Value = "Confirmed")]
        Confirmed,
        [EnumMember(Value = "Cancelled")]
        Cancelled
    }
}


