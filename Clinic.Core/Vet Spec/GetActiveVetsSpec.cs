namespace Clinic.Core.Vet_Spec
{
    public class GetActiveVetsSpec : BaseSpecifications<Vet, int>
    {
        public GetActiveVetsSpec() : base(v => v.ISActive)
        {
            AddInclude(v => v.Department); 
        }
    }
}
