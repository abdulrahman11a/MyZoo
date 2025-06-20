namespace Clinic.APIS.Controllers.v1
{
    public class VetByNameSpec : BaseSpecifications<Vet, int>
    {
        public VetByNameSpec(string name)
            : base(v => v.Name == name) // Use the criteria in the base class constructor
        {
        }
    }
}
