namespace Clinic.APIS.DTOS.AppointmentDto
{
    public class UpdateAppointmentDto
    {
        [FutureDate(ErrorMessage = "Appointment date must be in the future.")]
        public DateTime? AppointmentDate { get; set; }

        [StringLength(500, ErrorMessage = "Purpose cannot exceed 500 characters.")]
        public string? Purpose { get; set; }

        public AppointmentStatus? Status { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
        public int? PatientId { get; set; }

        [StringLength(100, ErrorMessage = "Vet name cannot exceed 100 characters.")]
        public string? VetName { get; set; }

        [StringLength(100, ErrorMessage = "Location name cannot exceed 100 characters.")]
        public string? LocationName { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date > DateTime.UtcNow;
            }
            return true; // Null is valid for nullable properties
        }
    }
}