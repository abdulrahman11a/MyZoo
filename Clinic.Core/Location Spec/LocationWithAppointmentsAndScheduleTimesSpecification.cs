namespace Clinic.Core.Location_Spec
{
    public class LocationWithAppointmentsAndScheduleTimesSpecification : BaseSpecifications<Location, int>
    {
        public LocationWithAppointmentsAndScheduleTimesSpecification() : base(null)
        {
            AddInclude(l => l.ScheduleTimes);
            AddInclude(l => l.Appointments);



        }
        public LocationWithAppointmentsAndScheduleTimesSpecification(int id) : base(l => l.Id==id)
        {
            AddInclude(l => l.ScheduleTimes);
            AddInclude(l => l.Appointments);



        }


    }
}
