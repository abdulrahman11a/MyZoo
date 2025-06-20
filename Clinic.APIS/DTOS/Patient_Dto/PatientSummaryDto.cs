namespace Clinic.APIS.DTOS.Patient_Dto
{
    public class PatientSummaryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalPrescriptions { get; set; }
        public int TotalNotifications { get; set; }
        public bool IsActive { get; set; }
    }
}