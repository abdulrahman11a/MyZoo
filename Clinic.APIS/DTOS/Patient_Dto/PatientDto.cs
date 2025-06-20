namespace Clinic.APIS.DTOS.Patient_Dto
{
    public class PatientDto
    {
        #region properties
            public int Id { get; set; } 
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public bool IsActive { get; set; }

        #endregion
    }

}