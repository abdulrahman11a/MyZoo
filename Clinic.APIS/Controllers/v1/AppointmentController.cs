namespace Clinic.APIS.Controllers.v1
{
    [ApiVersion("1.0")]
    [EnableRateLimiting("BookingLimiter")]
    public class AppointmentsController(IUnitOfWork _unitOfWork, IMapper _mapper, IAppointmentsService appointmentsService) : ApiBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var spec = new AppointmentsWithLocationAndPatientVetsSpecification();
            var appointments = await _unitOfWork.Repository<Appointment, int>().GetAllWithSpecAsync(spec);
            var appointmentsDto = _mapper.Map<IReadOnlyList<appointmentDto>>(appointments);

            var result = Result.Success(appointmentsDto);
            return result.ToActionResult();
        }
        [HttpGet("by-status")]
        public async Task<IActionResult> GetAppointmentsByStatus([FromQuery] string statuses = null)
        {
            var statusList = new List<AppointmentStatus>();
            if (string.IsNullOrWhiteSpace(statuses))
            {
                statusList.AddRange(Enum.GetValues(typeof(AppointmentStatus)).Cast<AppointmentStatus>());
            }
            else
            {
                var statusNames = statuses.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(s => s.Trim()).ToList();
                foreach (var statusName in statusNames)
                {
                    if (!Enum.TryParse(statusName, true, out AppointmentStatus parsedStatus) ||
                        !Enum.IsDefined(typeof(AppointmentStatus), parsedStatus))
                    {
                        return Result.Failure(new Error(400, $"Invalid status value: {statusName}."))
                                     .ToActionResult();
                    }
                    statusList.Add(parsedStatus);
                }
            }

            var spec = new AppointmentsByStatusSpecification(statusList);
            var appointments = await _unitOfWork.Repository<Appointment, int>().GetAllWithSpecAsync(spec);
            var appointmentsDto = _mapper.Map<IReadOnlyList<appointmentDto>>(appointments);

            if (!appointmentsDto.Any())
            {
                return Result.Failure(new Error(404, "No appointments found with the given status."))
                             .ToActionResult();
            }

            return Result.Success(appointmentsDto).ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var spec = new AppointmentsWithLocationAndPatientVetsSpecification(id);
            var appointment = await _unitOfWork.Repository<Appointment, int>().GetEntityWithSpecAsync(spec);

            if (appointment == null)
                return Result.Failure(new Error(404, $"Appointment with ID {id} not found.")).ToActionResult();

            var appointmentDto = _mapper.Map<appointmentDto>(appointment);
            return Result.Success(appointmentDto).ToActionResult();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return Result.Failure(new Error(400, "Invalid model data")).ToActionResult();

            var patientIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "patientId");
            if (patientIdClaim == null || !int.TryParse(patientIdClaim.Value, out int patientId))
                return Result.Failure(new Error(401, "Patient ID not found in user claims.")).ToActionResult();

            var vetSpec = new VetByNameSpec(requestDto.VetName);
            var vet = await _unitOfWork.Repository<Vet, int>().GetEntityWithSpecAsync(vetSpec);
            if (vet == null)
                return Result.Failure(new Error(400, $"Vet with name '{requestDto.VetName}' not found.")).ToActionResult();

            var locationSpec = new LocationByNameSpec(requestDto.LocationName);
            var location = await _unitOfWork.Repository<Location, int>().GetEntityWithSpecAsync(locationSpec);
            if (location == null)
                return Result.Failure(new Error(400, $"Location with name '{requestDto.LocationName}' not found.")).ToActionResult();

            if (!await _unitOfWork.Repository<Patient, int>().ExistsAsync(patientId))
                return Result.Failure(new Error(400, "Invalid Patient ID.")).ToActionResult();

            var appointmentTime = requestDto.AppointmentDate.TimeOfDay;
            var scheduleSpec = new CreateScheduleTimeByVetLocationAndTimeSpecification(vet.Id, location.Id, appointmentTime);
            var scheduleTime = await _unitOfWork.Repository<ScheduleTime, int>().GetEntityWithSpecAsync(scheduleSpec);

            if (scheduleTime == null)
                return Result.Failure(new Error(400, "No available schedule slot at the specified time.")).ToActionResult();

            var appointmentCount = await _unitOfWork.Repository<Appointment, int>()
                .GetCountAsync(new AppointmentsByScheduleTimeSpecification(scheduleTime));

            if (appointmentCount >= scheduleTime.Capacity)
                return Result.Failure(new Error(409, "Vet schedule slot is fully booked.")).ToActionResult();

            var locationAppointmentCount = await _unitOfWork.Repository<Appointment, int>()
                .GetCountAsync(new AppointmentsByLocationAndTimeSpecification(
                    location.Id, requestDto.AppointmentDate, requestDto.AppointmentDate.AddHours(1)));

            if (locationAppointmentCount >= location.Capacity)
                return Result.Failure(new Error(409, $"Location '{requestDto.LocationName}' is fully booked.")).ToActionResult();

            var newAppointment = new Appointment
            {
                AppointmentDate = requestDto.AppointmentDate,
                Purpose = requestDto.Purpose,
                PatientId = patientId,
                VetId = vet.Id,
                LocationId = location.Id,
                Status = AppointmentStatus.Pending
            };

            scheduleTime.Capacity--;
            location.Capacity--;

            await _unitOfWork.Repository<Appointment, int>().AddAsync(newAppointment);
            _unitOfWork.Repository<Location, int>().Update(location);
            _unitOfWork.Repository<ScheduleTime, int>().Update(scheduleTime);
            await _unitOfWork.CompleteAsync();

            var appointmentDto = _mapper.Map<appointmentDto>(newAppointment);
            return Result.Success(appointmentDto).ToActionResult();
        }
        [HttpPost("CreateAppointment-v2")]
        public async Task<IActionResult> CreateAppointmentv2([FromBody] CreateAppointmentRequestDto requestDto)
        {
            var patientSpec = new PatientByNameSpec(requestDto.Id);
            var patient = await _unitOfWork.Repository<Patient, int>().GetEntityWithSpecAsync(patientSpec);
            if (patient == null)
                return Result.Failure(new Error(404, $"Patient '{requestDto.Id}' not found")).ToActionResult();

            var vet = await _unitOfWork.Repository<Vet, int>().GetEntityWithSpecAsync(new VetByNameSpec(requestDto.VetName));
            if (vet == null)
                return Result.Failure(new Error(404, $"Vet '{requestDto.VetName}' not found")).ToActionResult();

            var location = await _unitOfWork.Repository<Location, int>().GetEntityWithSpecAsync(new LocationByNameSpec(requestDto.LocationName));
            if (location == null)
                return Result.Failure(new Error(404, $"Location '{requestDto.LocationName}' not found")).ToActionResult();

            var scheduleSpec = new CreateScheduleTimeByVetLocationAndTimeSpecification(
                vet.Id, location.Id, requestDto.AppointmentDate.TimeOfDay);
            var scheduleTime = await _unitOfWork.Repository<ScheduleTime, int>().GetEntityWithSpecAsync(scheduleSpec);
            if (scheduleTime == null)
                return Result.Failure(new Error(400, "No available schedule slot for the given time.")).ToActionResult();

            var appointmentCount = await _unitOfWork.Repository<Appointment, int>()
                .GetCountAsync(new AppointmentsByScheduleTimeSpecification(scheduleTime));
            if (appointmentCount >= scheduleTime.Capacity)
                return Result.Failure(new Error(409, "Vet's schedule slot is fully booked")).ToActionResult();

            var locationCount = await _unitOfWork.Repository<Appointment, int>()
                .GetCountAsync(new AppointmentsByLocationAndTimeSpecification(location.Id, requestDto.AppointmentDate, requestDto.AppointmentDate.AddHours(1)));
            if (locationCount >= location.Capacity)
                return Result.Failure(new Error(409, "Location is fully booked for the selected time")).ToActionResult();

            var newAppointment = new Appointment
            {
                AppointmentDate = requestDto.AppointmentDate,
                Purpose = requestDto.Purpose,
                PatientId = patient.Id,
                VetId = vet.Id,
                LocationId = location.Id,
                Status = AppointmentStatus.Pending
            };

            await _unitOfWork.Repository<Appointment, int>().AddAsync(newAppointment);

            // Optional: Update tracking (if needed for side-effects like notifications)
            await appointmentsService.UpdateAppointmentAsync(newAppointment);

            // Offload capacity updates via background task
            BackgroundJob.Enqueue<CapacityHelper>(x => x.DecrementCapacityAsync(location.Id, scheduleTime.Id));

            var appointmentDto = _mapper.Map<appointmentDto>(newAppointment);
            return Result.Success(appointmentDto).ToActionResult();
        }


        [Authorize(Roles = "Patient,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentDtoWithoutPatient updateDto)
        {
            if (!ModelState.IsValid)
                return Result.Failure(new Error(400, "Invalid appointment data")).ToActionResult();

            var appointment = await _unitOfWork.Repository<Appointment, int>().GetByIdAsync(id);
            if (appointment == null)
                return Result.Failure(new Error(404, $"Appointment with ID {id} not found")).ToActionResult();

            int? vetId = null, locationId = null;
            ScheduleTime newScheduleTime = null, originalScheduleTime = null;

            if (!string.IsNullOrWhiteSpace(updateDto.VetName))
            {
                var vet = await _unitOfWork.Repository<Vet, int>().GetEntityWithSpecAsync(new VetByNameSpec(updateDto.VetName));
                if (vet == null)
                    return Result.Failure(new Error(404, $"Vet '{updateDto.VetName}' not found")).ToActionResult();
                vetId = vet.Id;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.LocationName))
            {
                var location = await _unitOfWork.Repository<Location, int>().GetEntityWithSpecAsync(new LocationByNameSpec(updateDto.LocationName));
                if (location == null)
                    return Result.Failure(new Error(404, $"Location '{updateDto.LocationName}' not found")).ToActionResult();
                locationId = location.Id;
            }

            bool isTransfer = vetId.HasValue || locationId.HasValue || updateDto.AppointmentDate.HasValue;

            if (isTransfer)
            {
                var resolvedVetId = vetId ?? appointment.VetId;
                var resolvedLocationId = locationId ?? appointment.LocationId;
                var newDate = updateDto.AppointmentDate ?? appointment.AppointmentDate;

                var scheduleSpec = new ScheduleTimeByVetLocationAndTimeSpecification(
                    resolvedVetId, resolvedLocationId, newDate, newDate.AddHours(1));
                newScheduleTime = await _unitOfWork.Repository<ScheduleTime, int>().GetEntityWithSpecAsync(scheduleSpec);
                if (newScheduleTime == null)
                    return Result.Failure(new Error(400, "New schedule time not available")).ToActionResult();

                var newCount = await _unitOfWork.Repository<Appointment, int>()
                    .GetCountAsync(new AppointmentsByScheduleTimeSpecification(newScheduleTime));
                if (newCount >= newScheduleTime.Capacity)
                    return Result.Failure(new Error(409, "New schedule slot is full")).ToActionResult();

                var location = await _unitOfWork.Repository<Location, int>().GetByIdAsync(resolvedLocationId);
                var locationCount = await _unitOfWork.Repository<Appointment, int>()
                    .GetCountAsync(new AppointmentsByLocationAndTimeSpecification(resolvedLocationId, newDate, newDate.AddHours(1)));

                if (locationCount >= location.Capacity)
                    return Result.Failure(new Error(409, $"Location '{location.Name}' is fully booked")).ToActionResult();

                // Load original schedule to decrement capacity if needed
                originalScheduleTime = await _unitOfWork.Repository<ScheduleTime, int>()
                    .GetEntityWithSpecAsync(new ScheduleTimeByVetLocationAndTimeSpecification(
                        appointment.VetId, appointment.LocationId, appointment.AppointmentDate, appointment.AppointmentDate.AddHours(1)));
            }

            // Apply update
            appointment.AppointmentDate = updateDto.AppointmentDate ?? appointment.AppointmentDate;
            appointment.Purpose = updateDto.Purpose ?? appointment.Purpose;
            appointment.Status = updateDto.Status ?? appointment.Status;
            appointment.VetId = vetId ?? appointment.VetId;
            appointment.LocationId = locationId ?? appointment.LocationId;

            _unitOfWork.Repository<Appointment, int>().Update(appointment);

            if (isTransfer && originalScheduleTime != null)
            {
                var oldCount = await _unitOfWork.Repository<Appointment, int>()
                    .GetCountAsync(new AppointmentsByScheduleTimeSpecification(originalScheduleTime));
                if (oldCount <= 1)
                {
                    originalScheduleTime.Capacity++;
                    _unitOfWork.Repository<ScheduleTime, int>().Update(originalScheduleTime);
                }
            }

            await _unitOfWork.CompleteAsync();
            return Result.Success("Appointment updated successfully").ToActionResult();
        }


        [Authorize(Roles = "Patient,Admin")]
        [HttpPost("update-by-details")]
        public async Task<IActionResult> UpdateAppointmentByDetails([FromBody] UpdateAppointmentDtoWithoutPatient updateDto)
        {
            if (!ModelState.IsValid)
                return Result.Failure(new Error(400, "Invalid data.")).ToActionResult();

            var patientIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "patientId");
            if (patientIdClaim == null || !int.TryParse(patientIdClaim.Value, out int patientId))
                return Result.Failure(new Error(401, "Patient ID not found in user claims.")).ToActionResult();

            if (!updateDto.AppointmentDate.HasValue || string.IsNullOrWhiteSpace(updateDto.VetName) || string.IsNullOrWhiteSpace(updateDto.LocationName))
                return Result.Failure(new Error(400, "AppointmentDate, VetName, and LocationName are required to identify the appointment.")).ToActionResult();

            var vetSpec = new VetByNameSpec(updateDto.VetName);
            var vet = await _unitOfWork.Repository<Vet, int>().GetEntityWithSpecAsync(vetSpec);
            if (vet == null)
                return Result.Failure(new Error(404, $"Vet '{updateDto.VetName}' not found.")).ToActionResult();

            var locationSpec = new LocationByNameSpec(updateDto.LocationName);
            var location = await _unitOfWork.Repository<Location, int>().GetEntityWithSpecAsync(locationSpec);
            if (location == null)
                return Result.Failure(new Error(404, $"Location '{updateDto.LocationName}' not found.")).ToActionResult();

            var appointmentSpec = new AppointmentByDetailsSpecification(patientId, vet.Id, location.Id, updateDto.AppointmentDate.Value);
            var appointment = await _unitOfWork.Repository<Appointment, int>().GetEntityWithSpecAsync(appointmentSpec);
            if (appointment == null)
                return Result.Failure(new Error(404, "No matching appointment found.")).ToActionResult();

            // Re-check schedule availability
            var resolvedVetId = vet.Id;
            var resolvedLocationId = location.Id;
            var appointmentDate = updateDto.AppointmentDate.Value;

            var scheduleSpec = new ScheduleTimeByVetLocationAndTimeSpecification(resolvedVetId, resolvedLocationId, appointmentDate, appointmentDate.AddHours(1));
            var newSchedule = await _unitOfWork.Repository<ScheduleTime, int>().GetEntityWithSpecAsync(scheduleSpec);
            if (newSchedule == null)
                return Result.Failure(new Error(404, $"No available schedule slot for vet '{updateDto.VetName}' at location '{updateDto.LocationName}' on {appointmentDate}.")).ToActionResult();

            var appointmentCountSpec = new AppointmentsByScheduleTimeSpecification(newSchedule);
            var appointmentCount = await _unitOfWork.Repository<Appointment, int>().GetCountAsync(appointmentCountSpec);
            if (appointmentCount >= newSchedule.Capacity)
            {
                var conflicts = await _unitOfWork.Repository<Appointment, int>().GetAllWithSpecAsync(appointmentCountSpec);
                if (conflicts.Any(a => a.Id != appointment.Id))
                    return Result.Failure(new Error(409, "Schedule is fully booked.")).ToActionResult();
            }

            // Check location capacity
            var locationAppointmentSpec = new AppointmentsByLocationAndTimeSpecification(resolvedLocationId, appointmentDate, appointmentDate.AddHours(1));
            var locationAppointmentCount = await _unitOfWork.Repository<Appointment, int>().GetCountAsync(locationAppointmentSpec);
            if (locationAppointmentCount >= location.Capacity)
            {
                var conflicts = await _unitOfWork.Repository<Appointment, int>().GetAllWithSpecAsync(locationAppointmentSpec);
                if (conflicts.Any(a => a.Id != appointment.Id))
                    return Result.Failure(new Error(409, "Location is fully booked at this time.")).ToActionResult();
            }

            // Update logic
            appointment.AppointmentDate = appointmentDate;
            appointment.Purpose = updateDto.Purpose ?? appointment.Purpose;
            appointment.Status = updateDto.Status ?? appointment.Status;
            appointment.VetId = resolvedVetId;
            appointment.LocationId = resolvedLocationId;

            _unitOfWork.Repository<Appointment, int>().Update(appointment);

            await _unitOfWork.CompleteAsync();

            return Result.Success($"Appointment {appointment.Id} updated successfully.").ToActionResult();
        }


        [Authorize(Roles = "Patient,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _unitOfWork.Repository<Appointment, int>().GetByIdAsync(id);

            if (appointment == null)
                return Result.Failure(new Error(404, $"Appointment with ID {id} not found.")).ToActionResult();

            _unitOfWork.Repository<Appointment, int>().Delete(appointment);
            await _unitOfWork.CompleteAsync();

            return Result.Success($"Appointment {id} deleted successfully.").ToActionResult();
        }


        [Authorize(Roles = "Admin")]
        [HttpPatch("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeAppointmentStatus(int id, [FromBody] string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
                return Result.Failure(new Error(400, "Status must be provided.")).ToActionResult();

            if (!Enum.TryParse<AppointmentStatus>(newStatus, ignoreCase: true, out var parsedStatus) ||
                !Enum.IsDefined(typeof(AppointmentStatus), parsedStatus))
            {
                var validValues = string.Join(", ", Enum.GetNames(typeof(AppointmentStatus)));
                return Result.Failure(new Error(400, $"Invalid status '{newStatus}'. Valid values: {validValues}.")).ToActionResult();
            }

            var appointment = await _unitOfWork.Repository<Appointment, int>().GetByIdAsync(id);
            if (appointment == null)
                return Result.Failure(new Error(404, $"Appointment with ID {id} not found.")).ToActionResult();

            appointment.Status = parsedStatus;
            _unitOfWork.Repository<Appointment, int>().Update(appointment);
            await _unitOfWork.CompleteAsync();

            var updatedDto = _mapper.Map<appointmentDto>(appointment);
            return Result.Success(updatedDto).ToActionResult();
        }



    }
}
