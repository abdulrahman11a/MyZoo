namespace Clinic.APIS.DTOS.VetDtos
{
    public class VetCreateDto
    {
        #region Properties
        public string Name { get; set; }
        public bool ISActive { get; set; }
        public int DepartmentId { get; set; }
        public DateTime DateOfGraduation { get; set; }
        public string email { get; set; } = null!; 
        #endregion
    }
}
