namespace Clinic.Core.Location_Spec
{
    public class LocationWithAppointmentsSpecification : BaseSpecifications<Location, int>
    {
        public LocationWithAppointmentsSpecification(int id) : base(l => l.Id == id)
        {
            AddInclude(l => l.Appointments);
        }
    }
}