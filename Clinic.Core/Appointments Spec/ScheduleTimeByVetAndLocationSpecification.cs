namespace Clinic.Core.Appointments_Spec
{
    public class ScheduleTimeByVetAndLocationSpecification : BaseSpecifications<ScheduleTime,int>
    {
        public ScheduleTimeByVetAndLocationSpecification(
            int vetId,
            int locationId,
            DateTime startDate,
            DateTime endDate,
            List<DayOfWeek> daysOfWeek)
            : base(s => s.VetId == vetId &&
                        s.LocationId == locationId &&
                        s.StartTime >= startDate &&
                        s.EndTime <= endDate &&
                        daysOfWeek.Contains(s.StartTime.DayOfWeek))
        {
        }
    }
}