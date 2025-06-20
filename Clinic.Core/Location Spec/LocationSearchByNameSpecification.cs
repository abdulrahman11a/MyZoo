namespace Clinic.Core.Location_Spec
{
    public class LocationSearchByNameSpecification : BaseSpecifications<Location, int>
    {
        public LocationSearchByNameSpecification(string name) : base(loc => loc.Name.Contains(name))
        {
            AddInclude(loc => loc.Appointments);
            AddInclude(loc => loc.ScheduleTimes);
        }
    }

}
