namespace Clinic.APIS.DTOS
{
    public class ScheduleTimeDto
    {
        public int Id { get; set; }
        public int VetId { get; set; }
        public string VetName { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string ScheduleGroup { get; set; }
    }


    public class CreateScheduleTimeRequestDto
    {
        public string VetName { get; set; }
        public string LocationName { get; set; }

        // Accept only time strings like "01:00 PM"
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public int Capacity { get; set; }

   
        public int ScheduleGroup { get; set; }
    }

    public class UpdateScheduleTimeRequestDto
    {
        public string VetName { get; set; }
        public string LocationName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Capacity { get; set; }
        public int ScheduleGroup { get; set; }
    }
}