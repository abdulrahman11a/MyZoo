namespace Clinic.Core.Patient_Spec
{
    public class GetPatientswithAppointmentsWithVetSpec : BaseSpecifications<Patient, int>
    {
        public GetPatientswithAppointmentsWithVetSpec() : base(null)
        {

            AddInclude(p => p.Appointments);
            AddNavigationInclude("Appointments.Vet");
        }
        public GetPatientswithAppointmentsWithVetSpec(int id) : base(p => p.Id==id)
        {
            AddInclude(p => p.Appointments);
            AddNavigationInclude("Appointments.Vet");
        }

    }

}