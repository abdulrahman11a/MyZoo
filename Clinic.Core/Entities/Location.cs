namespace Clinic.Core.Entities
{
    public class Location : BaseEntity<int>
    {
        #region Properties
        public string Name { get; set; } = null!;
        public int Capacity { get; set; } = 1;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; } = true; // Indicates if this branch is open or not
        #endregion

        #region Relationships
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<ScheduleTime> ScheduleTimes { get; set; }
        #endregion
    }
}