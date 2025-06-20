namespace Clinic.Core.Appointments_Spec
{
    public class ScheduleTimeByVetLocationAndTimeSpecification : BaseSpecifications<ScheduleTime, int>
    {
        public ScheduleTimeByVetLocationAndTimeSpecification(int vetId, int locationId, DateTime appointmentStart, DateTime appointmentEnd)
            : base(s =>
                s.VetId == vetId &&
                s.LocationId == locationId &&
                appointmentStart.TimeOfDay >= s.StartTime.TimeOfDay &&
                appointmentEnd.TimeOfDay <= s.EndTime.TimeOfDay)
        {
        }
    }


}


public class CreateScheduleTimeByVetLocationAndTimeSpecification : BaseSpecifications<ScheduleTime, int>
{
    public CreateScheduleTimeByVetLocationAndTimeSpecification(int vetId, int locationId, TimeSpan appointmentTime)
        : base(s =>
            s.VetId == vetId &&
            s.LocationId == locationId &&
            appointmentTime >= s.StartTime.TimeOfDay &&
            appointmentTime < s.EndTime.TimeOfDay)
    {
    }
}