namespace Clinic.APIS.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ScheduleTimeController(IUnitOfWork _unitOfWork, IMapper _mapper) : ApiBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetScheduleTimes()
        {
            var spec = new ScheduleTimeWithVetAndLocationSpecification();
            var scheduleTimes = await _unitOfWork.Repository<ScheduleTime, int>().GetAllWithSpecAsync(spec);
            var result = Result.Success(_mapper.Map<IReadOnlyList<ScheduleTimeDto>>(scheduleTimes));
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleTime(int id)
        {
            var spec = new ScheduleTimeWithVetAndLocationSpecification(id);
            var scheduleTime = await _unitOfWork.Repository<ScheduleTime, int>().GetEntityWithSpecAsync(spec);
            var result = scheduleTime is null
                ? Result.Failure<ScheduleTimeDto>(new Error(404, $"ScheduleTime with ID {id} not found."))
                : Result.Success(_mapper.Map<ScheduleTimeDto>(scheduleTime));
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [CachedAttribute(600)]
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableScheduleTimes()
        {
            var scheduleSpec = new ScheduleTimeByVetLocationAndDateRangeSpecification();
            var scheduleTimes = await _unitOfWork.Repository<ScheduleTime, int>().GetAllWithSpecAsync(scheduleSpec);
            var result = Result.Success(_mapper.Map<IReadOnlyList<ScheduleTimeDto>>(scheduleTimes));
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost]
        public async Task<IActionResult> CreateScheduleTime([FromBody] CreateScheduleTimeRequestDto requestDto)
        {
            var vetSpec = new VetByNameSpec(requestDto.VetName);
            var vet = await _unitOfWork.Repository<Vet, int>().GetEntityWithSpecAsync(vetSpec);
            if (vet == null)
                return Result.Failure<string>(new Error(400, $"Vet with name '{requestDto.VetName}' not found.")).ToProblem();

            var locationSpec = new LocationByNameSpec(requestDto.LocationName);
            var location = await _unitOfWork.Repository<Location, int>().GetEntityWithSpecAsync(locationSpec);
            if (location == null)
                return Result.Failure<string>(new Error(400, $"Location with name '{requestDto.LocationName}' not found.")).ToProblem();

            if (!DateTime.TryParse(requestDto.StartTime, out var parsedStartTime))
                return Result.Failure<string>(new Error(400, "Invalid StartTime format.")).ToProblem();

            if (!DateTime.TryParse(requestDto.EndTime, out var parsedEndTime))
                return Result.Failure<string>(new Error(400, "Invalid EndTime format.")).ToProblem();

            var today = DateTime.Today;
            var startTime = today.Add(parsedStartTime.TimeOfDay);
            var endTime = today.Add(parsedEndTime.TimeOfDay);

            if (startTime >= endTime)
                return Result.Failure<string>(new Error(400, "StartTime must be earlier than EndTime.")).ToProblem();

            if (requestDto.Capacity <= 0)
                return Result.Failure<string>(new Error(400, "Capacity must be greater than 0.")).ToProblem();

            if (!Enum.IsDefined(typeof(ScheduleGroup), requestDto.ScheduleGroup))
                return Result.Failure<string>(new Error(400, "Invalid schedule group number.")).ToProblem();

            var scheduleTime = new ScheduleTime
            {
                VetId = vet.Id,
                LocationId = location.Id,
                StartTime = startTime,
                EndTime = endTime,
                Capacity = requestDto.Capacity,
                ScheduleGroup = (ScheduleGroup)requestDto.ScheduleGroup
            };

            await _unitOfWork.Repository<ScheduleTime, int>().AddAsync(scheduleTime);
            await _unitOfWork.CompleteAsync();

            var scheduleTimeDto = _mapper.Map<ScheduleTimeDto>(scheduleTime);
            scheduleTimeDto.VetName = vet.Name;
            scheduleTimeDto.LocationName = location.Name;

            return CreatedAtAction(nameof(GetScheduleTime), new { id = scheduleTime.Id }, scheduleTimeDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScheduleTime(int id, [FromBody] UpdateScheduleTimeRequestDto updateDto)
        {
            var scheduleTime = await _unitOfWork.Repository<ScheduleTime, int>().GetByIdAsync(id);
            if (scheduleTime == null)
                return Result.Failure<string>(new Error(404, $"ScheduleTime with ID {id} not found.")).ToProblem();

            int? vetId = null;
            int? locationId = null;

            if (!string.IsNullOrWhiteSpace(updateDto.VetName))
            {
                var vetSpec = new VetByNameSpec(updateDto.VetName);
                var vet = await _unitOfWork.Repository<Vet, int>().GetEntityWithSpecAsync(vetSpec);
                if (vet == null)
                    return Result.Failure<string>(new Error(400, $"Vet with name '{updateDto.VetName}' not found.")).ToProblem();
                vetId = vet.Id;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.LocationName))
            {
                var locationSpec = new LocationByNameSpec(updateDto.LocationName);
                var location = await _unitOfWork.Repository<Location, int>().GetEntityWithSpecAsync(locationSpec);
                if (location == null)
                    return Result.Failure<string>(new Error(400, $"Location with name '{updateDto.LocationName}' not found.")).ToProblem();
                locationId = location.Id;
            }

            if (!DateTime.TryParseExact(updateDto.StartTime, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedStartTime) ||
                !DateTime.TryParseExact(updateDto.EndTime, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedEndTime))
            {
                return Result.Failure<string>(new Error(400, "Time format must be 'hh:mm tt' (e.g., '08:00 AM').")).ToProblem();
            }

            var today = DateTime.Today;
            var startTime = today.Add(parsedStartTime.TimeOfDay);
            var endTime = today.Add(parsedEndTime.TimeOfDay);

            if (startTime >= endTime)
                return Result.Failure<string>(new Error(400, "StartTime must be earlier than EndTime.")).ToProblem();

            if (updateDto.Capacity <= 0)
                return Result.Failure<string>(new Error(400, "Capacity must be greater than 0.")).ToProblem();

            scheduleTime.VetId = vetId ?? scheduleTime.VetId;
            scheduleTime.LocationId = locationId ?? scheduleTime.LocationId;
            scheduleTime.StartTime = startTime;
            scheduleTime.EndTime = endTime;
            scheduleTime.Capacity = updateDto.Capacity;
            scheduleTime.ScheduleGroup = (ScheduleGroup)updateDto.ScheduleGroup;

            _unitOfWork.Repository<ScheduleTime, int>().Update(scheduleTime);
            await _unitOfWork.CompleteAsync();

            return Result.Success("ScheduleTime updated successfully.").ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduleTime(int id)
        {
            var scheduleTime = await _unitOfWork.Repository<ScheduleTime, int>().GetByIdAsync(id);
            if (scheduleTime == null)
                return Result.Failure<string>(new Error(404, $"ScheduleTime with ID {id} not found.")).ToProblem();

            var appointmentCountSpec = new AppointmentsByScheduleTimeSpecification(scheduleTime);
            var appointmentCount = await _unitOfWork.Repository<Appointment, int>().GetCountAsync(appointmentCountSpec);
            if (appointmentCount > 0)
                return Result.Failure<string>(new Error(400, $"Cannot delete schedule slot with {appointmentCount} active appointments.")).ToProblem();

            _unitOfWork.Repository<ScheduleTime, int>().Delete(scheduleTime);
            await _unitOfWork.CompleteAsync();

            return Result.Success($"ScheduleTime with ID {id} deleted successfully.").ToActionResult();
        }
    }
}
