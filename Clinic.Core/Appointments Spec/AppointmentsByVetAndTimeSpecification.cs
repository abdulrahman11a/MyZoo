namespace Clinic.Core.Appointments_Spec
{
    public class AppointmentsByVetAndTimeSpecification : BaseSpecifications<Appointment, int>
    {
        public AppointmentsByVetAndTimeSpecification(int vetId, DateTime appointmentDate)
            : base(a => a.VetId == vetId && a.AppointmentDate == appointmentDate)
        {
            // No includes needed for conflict checking
        }
    }
}