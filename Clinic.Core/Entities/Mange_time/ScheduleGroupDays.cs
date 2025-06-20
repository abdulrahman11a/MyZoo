namespace Clinic.Core.Entities.Mange_time
{

public static class ScheduleGroupDays
{
    private static readonly Dictionary<ScheduleGroup, List<DayOfWeek>> GroupDays = new Dictionary<ScheduleGroup, List<DayOfWeek>>
        {
            { ScheduleGroup.Group1, new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday } },
            { ScheduleGroup.Group2, new List<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday } },
            { ScheduleGroup.Group3, new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday } }
        };

    public static List<DayOfWeek> GetDaysForGroup(ScheduleGroup group) => GroupDays[group];
    public static bool IsDayInGroup(DayOfWeek day, ScheduleGroup group) => GroupDays[group].Contains(day);
}

}