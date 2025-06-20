namespace Clinic.Core.ScheduleTime_Spec
{
    public class ScheduleTimeWithVetAndLocationSpecification : BaseSpecifications<ScheduleTime,int>
    {
        public ScheduleTimeWithVetAndLocationSpecification()
            : base(null)
        {
            AddInclude(st => st.Vet);
            AddInclude(st => st.Location);
        }

        public ScheduleTimeWithVetAndLocationSpecification(int id)
            : base(st => st.Id == id)
        {
            AddInclude(st => st.Vet);
            AddInclude(st => st.Location);
        }
    }
}