namespace Clinic.Core.Appointments_Spec
{
    public class AppointmentByDetailsSpecification : BaseSpecifications<Appointment,int>
    {
        public AppointmentByDetailsSpecification(int patientId, int vetId, int locationId, DateTime appointmentDate)
            : base(a => a.PatientId == patientId &&
                        a.VetId == vetId &&
                        a.LocationId == locationId &&
                        a.AppointmentDate == appointmentDate &&
                        a.Status != AppointmentStatus.Cancelled)
        {
        }
    }
}