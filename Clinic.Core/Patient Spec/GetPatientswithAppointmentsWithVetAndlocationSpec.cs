namespace Clinic.Core.Patient_Spec
{
    public class GetPatientswithAppointmentsWithVetAndlocationSpec : BaseSpecifications<Patient, int>
    {
        public GetPatientswithAppointmentsWithVetAndlocationSpec() : base(null)
        {

            AddInclude(p => p.Appointments);
            AddNavigationInclude("Appointments.Vet");
            AddNavigationInclude("Appointments.Location");

        }
        public GetPatientswithAppointmentsWithVetAndlocationSpec(int id) : base(p => p.Id==id)
        {
            AddInclude(p => p.Appointments);
            AddNavigationInclude("Appointments.Vet");
            AddNavigationInclude("Appointments.Location");
        }

    }
}