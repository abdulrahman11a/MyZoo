namespace Clinic.APIS.DTOS.Patient_Dto
{
    public class PatientWithAppointmentsVetAndLocationDto:PatientDto
    {

        public ICollection<appointmentDtoforPatient> Appointments { get; set; }


    }
}