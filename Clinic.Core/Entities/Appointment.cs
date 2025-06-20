namespace Clinic.Core.Entities
{
    public class Appointment : BaseEntity<int>
    {
        #region Properties
        public DateTime AppointmentDate { get; set; }
        public string Purpose { get; set; } = null!;
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        #endregion
        #region Relationships
        public Patient Patient { get; set; }
        public Vet Vet { get; set; }
        public Location Location { get; set; } 
        #endregion

        #region Foreignkeys
        public int PatientId { get; set; }
        public int VetId { get; set; }
        public int LocationId { get; set; } 
        #endregion
    }
}