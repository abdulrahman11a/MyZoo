namespace Clinic.APIS.DTOS.VetDtos
{
    public class GetVetsWithScheduleTimesDto
    {

        #region Properties

        public string Name { get; set; }
        public int Age { get; set; }
        public string email { get; set; } = null!;
        public DateTime DateOfGraduation { get; set; }
        #endregion


        #region Relationships 
        public ICollection<ScheduleTimeDto> ScheduleTimes { get; set; }
        public string Department { get; set; }
        #endregion

    }
}