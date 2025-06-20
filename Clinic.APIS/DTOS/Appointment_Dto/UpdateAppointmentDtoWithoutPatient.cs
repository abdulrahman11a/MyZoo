namespace Clinic.APIS.DTOS.AppointmentDto
{
    public class UpdateAppointmentDtoWithoutPatient
    {
        public DateTime?AppointmentDate { get; set; }

        [StringLength(500, ErrorMessage = "Purpose cannot exceed 500 characters.")]
        public string? Purpose { get; set; }

        public AppointmentStatus? Status { get; set; }

        [StringLength(100, ErrorMessage = "Vet name cannot exceed 100 characters.")]
        public string? VetName { get; set; }

        [StringLength(100, ErrorMessage = "Location name cannot exceed 100 characters.")]
        public string? LocationName { get; set; }
    }


}