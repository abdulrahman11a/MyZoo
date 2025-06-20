namespace Clinic.APIS.DTOS.VetDtos
{
    public class VetUpdateDto
    {
        #region Properties
        public string Name { get; set; }

        public bool ISActive { get; set; }

        public int DepartmentId { get; set; }

        public string email { get; set; } = null!; 
        #endregion
    }
}