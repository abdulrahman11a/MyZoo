namespace Clinic.Core.Vet_Spec
{
    public class GetVetByNameSpec : BaseSpecifications<Vet,int>
    {
        public GetVetByNameSpec(string name)
            : base(vet => vet.Name == name)
        {
        }
    }

}