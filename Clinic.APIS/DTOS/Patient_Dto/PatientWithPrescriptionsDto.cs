using Clinic.APIS.DTOS.Prescription_DTO;

namespace Clinic.APIS.DTOS.Patient_Dto
{
    public class PatientWithPrescriptionsDto:PatientDto
    {
        public ICollection<PrescriptionDto> Prescriptions { get; set; }

    }
}