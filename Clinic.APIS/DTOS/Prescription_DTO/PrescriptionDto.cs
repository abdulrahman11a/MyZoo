namespace Clinic.APIS.DTOS.Prescription_DTO
{
    public class PrescriptionDto
    {
        #region Properties
        public int Id { get; set; } 

        public string MedicationName { get; set; } = null!;

        public string Dosage { get; set; } = null!;

        public string Frequency { get; set; } = null!;

        public string Instructions { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        #endregion

        #region Relationships

        public string PatientName { get; set; } = null!;

        public string VetName { get; set; } = null!;

        #endregion
    }
}
