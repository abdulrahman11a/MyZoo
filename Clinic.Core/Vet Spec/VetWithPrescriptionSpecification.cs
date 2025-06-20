namespace Clinic.Core.Vet_Spec
{
    public class GetVetsWithPrescriptionsSpec : BaseSpecifications<Vet, int>
    {
        public GetVetsWithPrescriptionsSpec() : base(null)
        {
            AddInclude(v => v.Prescriptions); 
            AddInclude(v => v.Department);
            AddNavigationInclude("Prescriptions.Patient");

        }

        public GetVetsWithPrescriptionsSpec(int id) : base(v => v.Id == id)
        {
            AddInclude(v => v.Prescriptions);
            AddInclude(v => v.Department);
            AddNavigationInclude("Prescriptions.Patient");
        }
    }
}