namespace Clinic.Core.Entities
{
    public class Vet : BaseEntity<int>
    {
        #region Properties
        public string Name { get; set; }
        public int Age { get; set; }
        public string email { get; set; } = null!;
        public DateTime DateOfGraduation { get; set; }
        public bool ISActive { get; set; }=true;

        #endregion

        #region ForeignKeys
        public int DepartmentId { get; set; }
        #endregion

        #region Relationships 
        public ICollection<ScheduleTime> ScheduleTimes { get; set; } = new List<ScheduleTime>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>(); 
        public Department Department { get; set; }
        #endregion
    }
}