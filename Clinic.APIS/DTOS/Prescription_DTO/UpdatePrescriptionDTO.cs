namespace Clinic.APIS.DTOS.Prescription_DTO
{
    public class UpdatePrescriptionDTO
    {
        #region Properties

        public string MedicationName { get; set; } = null!;

        public string Dosage { get; set; } = null!;

        public string Frequency { get; set; } = null!;

        public string Instructions { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        #endregion

        #region Foreign Keys

        public int PatientId { get; set; }

        public int VetId { get; set; }

        #endregion
    }
}
