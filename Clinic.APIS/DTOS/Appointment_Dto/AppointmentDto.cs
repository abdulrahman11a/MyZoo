namespace Clinic.APIS.DTOS.Appointment_Dto
{
    public class appointmentDto
    {

        #region Properties
        public DateTime AppointmentDate { get; set; }
        public string FormattedDate => AppointmentDate.ToString("dd-MM-yyyy HH:mm");
        public string Purpose { get; set; } = null!;
        public AppointmentStatus Status { get; set; }
        #endregion

        #region Relationships
        public string Patient { get; set; }
        public string Vet { get; set; }
        public string Location { get; set; }
        #endregion

    }
}
namespace Clinic.APIS.DTOS.Appointment_Dto
{
    public class appointmentDtoforPatient
    {

        #region Properties
        public DateTime AppointmentDate { get; set; }
        public string FormattedDate => AppointmentDate.ToString("dd-MM-yyyy HH:mm");
        public string Purpose { get; set; } = null!;
        public AppointmentStatus Status { get; set; }
        #endregion

        #region Relationships
        public string Vet { get; set; }
        public string Location { get; set; }
        #endregion

    }
}
