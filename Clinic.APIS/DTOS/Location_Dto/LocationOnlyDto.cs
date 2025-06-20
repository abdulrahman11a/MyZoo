namespace Clinic.APIS.DTOS.Location_Dto
{
    public class LocationOnlyDto
    {
        #region Properties
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; } = true; // Indicates if this branch is open or not
        #endregion
    }
}
