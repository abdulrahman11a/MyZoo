namespace Clinic.Core.Vet_Spec
{
    public class VetWithScheduleTimesSpecification : BaseSpecifications<Vet, int>
    {
        public VetWithScheduleTimesSpecification() : base(null)
        {
            AddInclude(v => v.ScheduleTimes);
            AddInclude(v => v.Department);

            AddNavigationInclude("ScheduleTimes.Location"); // الصح بدون v.
        }

        public VetWithScheduleTimesSpecification(int id) : base(v => v.Id == id)
        {
            AddInclude(v => v.ScheduleTimes);
            AddInclude(v => v.Department); // مهم هنا برضو
            AddNavigationInclude("ScheduleTimes.Location");
        }
    }
}
