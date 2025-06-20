namespace Clinic.Core.Location_Spec
{

    public class LocationByVetSpecification : BaseSpecifications<Location, int>
    {
        public LocationByVetSpecification(int vetId) : base(loc =>
             loc.ScheduleTimes.Any(st => st.VetId == vetId))
        {
            AddInclude(loc => loc.Appointments);
            AddInclude(loc => loc.ScheduleTimes);
            AddNavigationInclude("Appointments.Vet");
            AddNavigationInclude("Appointments.Patient");
        }
    }

} 