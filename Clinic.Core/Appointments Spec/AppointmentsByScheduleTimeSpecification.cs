namespace Clinic.Core.Appointments_Spec
{
    public class AppointmentsByScheduleTimeSpecification : BaseSpecifications<Appointment,int>
    {
        public AppointmentsByScheduleTimeSpecification(ScheduleTime scheduleTime)
              : base(a => a.VetId == scheduleTime.VetId &&
                          a.LocationId == scheduleTime.LocationId &&
                          a.AppointmentDate >= scheduleTime.StartTime &&
                          a.AppointmentDate < scheduleTime.EndTime &&
                          a.Status != AppointmentStatus.Cancelled) // Exclude cancelled appointments
        {
        }
    }
}