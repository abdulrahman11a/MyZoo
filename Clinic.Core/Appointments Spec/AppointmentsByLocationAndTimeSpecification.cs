namespace Clinic.Core.Appointments_Spec
{
    public class AppointmentsByLocationAndTimeSpecification : BaseSpecifications<Appointment,int>
    {
        public AppointmentsByLocationAndTimeSpecification(int locationId, DateTime startTime, DateTime endTime)
            : base(a => a.LocationId == locationId &&
                        a.AppointmentDate >= startTime &&
                        a.AppointmentDate < endTime &&
                        a.Status != AppointmentStatus.Cancelled)
        {
        }
    }
}