namespace Clinic.APIS.DTOS.Patient_Dto
{
    public class PatientWithNotificationsDto:PatientDto
    {
        public ICollection<NotificationDto> Notifications { get; set; }

    }
}   