namespace Clinic.Core.Entities
{
    public class ScheduleTime : BaseEntity<int>
    {
        #region Properties
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; } = 1;//The maximum number of appointments the vet can handle in this time slot at this location (before any dynamic increments)

        public ScheduleGroup ScheduleGroup { get; set; }
        #endregion

        #region Relationships
        public Vet Vet { get; set; }
        public Location Location { get; set; }
        #endregion

        #region ForeignKeys
        public int VetId { get; set; }
        public int LocationId { get; set; }
        #endregion

    }
}