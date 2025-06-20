namespace Clinic.Core.Appointments_Spec
{
    public class LocationByNameSpec : BaseSpecifications<Location, int>
    {
        public LocationByNameSpec(string name)
            : base(l => l.Name == name) // Specify criteria to search by name
        {
        }
    }
}
