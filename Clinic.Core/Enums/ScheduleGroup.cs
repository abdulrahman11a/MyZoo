namespace Clinic.Core.Enums
{
    public enum ScheduleGroup
    {
        [EnumMember(Value = "Monday,Tuesday,Wednesday")]
        Group1, // Monday, Tuesday, Wednesday
        [EnumMember(Value = "Thursday,Friday")]
        Group2, // Thursday, Friday
        [EnumMember(Value = "Saturday,Sunday")]
        Group3  // Saturday, Sunday
    }
}
