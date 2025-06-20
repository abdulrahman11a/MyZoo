namespace Clinic.Core.Entities
{
    public class Prescription : BaseEntity<int>
    {
        #region Properties
        public string MedicationName { get; set; } = null!;

        public string Dosage { get; set; } = null!;

        public string Frequency { get; set; } = null!;

        public string Instructions { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        #endregion

        #region Relationships
        public Patient Patient { get; set; }
        public Vet Vet { get; set; }
        #endregion

       
        #region Foreignkeys
        public int PatientId { get; set; }

        public int VetId { get; set; }
        #endregion
    }
}
