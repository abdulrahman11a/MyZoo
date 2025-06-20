namespace Clinic.APIS.DTOS.VetDtos
{
    public class GetVetsWithNotificationsDto
    {
        #region Properties
        public string Name { get; set; }
 
        #endregion

        #region Relationships
        public ICollection<NotificationDto> Notifications { get; set; }
        public string Department { get; set; }
        #endregion
    }
}
