public class GetPatientWithPrescriptionsSpec : BaseSpecifications<Patient, int>
    {
        public GetPatientWithPrescriptionsSpec() : base(null)
        {

            AddInclude(p => p.Prescriptions);
         
        }
        public GetPatientWithPrescriptionsSpec(int id) : base(p => p.Id==id)
        {
            AddInclude(p => p.Prescriptions);
        }

   }

