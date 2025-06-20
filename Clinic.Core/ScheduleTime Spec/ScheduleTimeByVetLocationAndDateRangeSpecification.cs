namespace Clinic.Core.ScheduleTime_Spec
{ 
    public class ScheduleTimeByVetLocationAndDateRangeSpecification : BaseSpecifications<ScheduleTime,int>
    {
        public ScheduleTimeByVetLocationAndDateRangeSpecification()
            : base( st=>st.Capacity!=0&&st.Location.Capacity!=0)
        {
            AddInclude(st => st.Vet);
            AddInclude(st => st.Location);
        }
    }
}

