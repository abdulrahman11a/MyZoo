namespace Clinic.Core.Vet_Spec
{
    public class VetWithNotificationSpecification : BaseSpecifications<Vet, int>
    {
        public VetWithNotificationSpecification() : base(null)
        {
            AddInclude(v => v.Notifications);
            AddInclude(v => v.Department);
        }

        public VetWithNotificationSpecification(int id) : base(v => v.Id == id)
        {
            AddInclude(v => v.Notifications);
        }
    }
}
