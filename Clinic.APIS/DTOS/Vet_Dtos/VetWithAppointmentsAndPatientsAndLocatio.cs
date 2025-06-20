namespace Clinic.APIS.DTOS.VetDtos
{
    public class VetWithAppointmentsAndPatientsAndLocatioDto
    {
            #region Properties
            public string Name { get; set; }
            public int Age { get; set; }
            public string email { get; set; } = null!;
            public DateTime DateOfGraduation { get; set; }
            #endregion

            #region Relationships 
            public ICollection<appointmentDto> Appointments { get; set;  }
            public string Department { get; set; }
            #endregion
    }
}