namespace Clinic.Core.Appointments_Spec
{
    public class AppointmentsWithLocationAndPatientVetsSpecification:BaseSpecifications<Appointment,int>
    {
        public AppointmentsWithLocationAndPatientVetsSpecification():base(null)
        {
            AddInclude(a=>a.Location);
            AddInclude(a=>a.Patient);
            AddInclude(a=>a.Vet);


        }

        public AppointmentsWithLocationAndPatientVetsSpecification(int id) : base(a=> a.Id==id)
        {
            AddInclude(a => a.Location);
            AddInclude(a => a.Patient);
            AddInclude(a => a.Vet);


        }

    }
}
