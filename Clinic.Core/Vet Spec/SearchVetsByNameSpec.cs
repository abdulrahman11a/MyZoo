namespace Clinic.Core.Vet_Spec
{
    public class SearchVetsByNameSpec : BaseSpecifications<Vet, int>
    {
        public SearchVetsByNameSpec(string name)
            : base(v => v.Name.Contains(name))
        {
            AddInclude(v => v.Department); 
        }
    }
}
