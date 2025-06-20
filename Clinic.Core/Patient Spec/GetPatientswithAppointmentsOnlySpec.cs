namespace Clinic.Core.Patient_Spec
{
    public  class GetPatientswithAppointmentsOnlySpec : BaseSpecifications<Patient, int>
    {
        public GetPatientswithAppointmentsOnlySpec():base(null){


            AddInclude(p=>p.Appointments);
            AddNavigationInclude("Appointments.Vet");
            AddNavigationInclude("Appointments.Location");



        }
        public GetPatientswithAppointmentsOnlySpec(int id) : base(p=>p.Id==id)
        {
            AddInclude(p => p.Appointments);
            AddNavigationInclude("Appointments.Vet");
            AddNavigationInclude("Appointments.Location");

        }

    }
}

