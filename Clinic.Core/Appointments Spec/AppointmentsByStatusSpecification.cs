namespace Clinic.Core.Appointments_Spec
{
    public class AppointmentsByStatusSpecification : BaseSpecifications<Appointment,int>

    {
        public AppointmentsByStatusSpecification(List<AppointmentStatus> statuses)
            : base(a => statuses.Contains(a.Status))
        {
            AddInclude(a => a.Location);
            AddInclude(a => a.Patient);
            AddInclude(a => a.Vet);
        }
    }
}