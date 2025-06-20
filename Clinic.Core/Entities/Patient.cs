namespace Clinic.Core.Entities
{
    public class Patient : BaseEntity<int>
    {
        #region properties

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool ISActive { get; set; }=true;
        #endregion

        #region Relationships
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        #endregion
    }
}