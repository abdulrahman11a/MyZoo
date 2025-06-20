namespace Clinic.APIS.DTOS.AppointmentDto
{
    public class CreateAppointmentRequestDto
    {
        [Required(ErrorMessage = "Full name is required.")]
       // public string FullName { get; set; }
          public int Id { get; set; }  
        [Required(ErrorMessage = "Appointment date is required.")]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Purpose { get; set; } = string.Empty;

        [Required]
        public string VetName { get; set; } = string.Empty;

        [Required]
        public string LocationName { get; set; } = string.Empty;
    }
}
