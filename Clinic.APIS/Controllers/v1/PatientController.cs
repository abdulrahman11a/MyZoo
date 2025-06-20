namespace Clinic.APIS.Controllers.v1
{
    public class PatientController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _unitOfWork.Repository<Patient, int>().GetAllAsync();
            var dto = _mapper.Map<IReadOnlyList<PatientDto>>(patients);
            return Result.Success(dto).ToActionResult();
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActivePatients()
        {
            var spec = new GetActivePatientsSpec();
            var patients = await _unitOfWork.Repository<Patient, int>().GetAllWithSpecAsync(spec);
            var dto = _mapper.Map<IReadOnlyList<PatientDto>>(patients);
            return Result.Success(dto).ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var patient = await _unitOfWork.Repository<Patient, int>().GetByIdAsync(id);
            if (patient == null)
                return Result.Failure<PatientDto>(new Error(404, "Patient not found")).ToActionResult();

            var dto = _mapper.Map<PatientDto>(patient);
            return Result.Success(dto).ToActionResult();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPatientsByName([FromQuery, Required] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<IReadOnlyList<PatientDto>>(new Error(400, "Search query is required")).ToActionResult();

            var spec = new SearchPatientsByNameSpec(name);
            var patients = await _unitOfWork.Repository<Patient, int>().GetAllWithSpecAsync(spec);
            var dto = _mapper.Map<IReadOnlyList<PatientDto>>(patients);
            return Result.Success(dto).ToActionResult();
        }

        [HttpGet("with-appointments")]
        public async Task<IActionResult> GetPatientsWithAppointments()
        {
            var spec = new GetPatientswithAppointmentsOnlySpec();
            var patients = await _unitOfWork.Repository<Patient, int>().GetAllWithSpecAsync(spec);
            var dto = _mapper.Map<IReadOnlyList<PatientWithAppointmentsVetAndLocationDto>>(patients);
            return Result.Success(dto).ToActionResult();
        }

        [HttpGet("{id}/with-prescriptions")]
        public async Task<IActionResult> GetPatientWithPrescriptions(int id)
        {
            var spec = new GetPatientWithPrescriptionsSpec(id);
            var patient = await _unitOfWork.Repository<Patient, int>().GetEntityWithSpecAsync(spec);
            if (patient == null)
                return Result.Failure<PatientWithPrescriptionsDto>(new Error(404, "Patient not found")).ToActionResult();

            var dto = _mapper.Map<PatientWithPrescriptionsDto>(patient);
            return Result.Success(dto).ToActionResult();
        }

        [HttpGet("{id}/with-notifications")]
        public async Task<IActionResult> GetPatientWithNotifications(int id)
        {
            var spec = new GetPatientWithNotificationsSpec(id);
            var patient = await _unitOfWork.Repository<Patient, int>().GetEntityWithSpecAsync(spec);
            if (patient == null)
                return Result.Failure<PatientWithNotificationsDto>(new Error(404, "Patient not found")).ToActionResult();

            var dto = _mapper.Map<PatientWithNotificationsDto>(patient);
            return Result.Success(dto).ToActionResult();
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return Result.Failure(new Error(400, "Invalid input")).ToActionResult();

            var patient = _mapper.Map<Patient>(requestDto);
            await _unitOfWork.Repository<Patient, int>().AddAsync(patient);
            await _unitOfWork.CompleteAsync();

            var dto = _mapper.Map<PatientDto>(patient);
            return Result.Success(dto).ToActionResult();
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return Result.Failure(new Error(400, "Invalid input")).ToActionResult();

            var patient = await _unitOfWork.Repository<Patient, int>().GetByIdAsync(id);
            if (patient == null)
                return Result.Failure(new Error(404, "Patient not found")).ToActionResult();

            _mapper.Map(updateDto, patient);
            _unitOfWork.Repository<Patient, int>().Update(patient);
            await _unitOfWork.CompleteAsync();

            return Result.Success().ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _unitOfWork.Repository<Patient, int>().GetByIdAsync(id);
            if (patient == null)
                return Result.Failure(new Error(404, "Patient not found")).ToActionResult();

            if (patient.Appointments.Any(a => a.Status != AppointmentStatus.Cancelled))
                return Result.Failure(new Error(400, "Cannot delete patient with active appointments")).ToActionResult();

            _unitOfWork.Repository<Patient, int>().Delete(patient);
            await _unitOfWork.CompleteAsync();

            return Result.Success().ToActionResult();
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetPatientSummaries()
        {
            var patients = await _unitOfWork.Repository<Patient, int>().GetAllAsync();
            var dto = _mapper.Map<IReadOnlyList<PatientSummaryDto>>(patients);
            return Result.Success(dto).ToActionResult();
        }

        [HttpGet("{id}/upcoming-appointments")]
        public async Task<IActionResult> GetUpcomingAppointments(int id)
        {
            var spec = new GetPatientswithAppointmentsWithVetAndlocationSpec(id);
            var patient = await _unitOfWork.Repository<Patient, int>().GetEntityWithSpecAsync(spec);
            if (patient == null)
                return Result.Failure<IReadOnlyList<appointmentDtoforPatient>>(new Error(404, "Patient not found")).ToActionResult();

            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            var upcomingAppointments = patient.Appointments
                .Where(a => a.AppointmentDate >= now && a.Status != AppointmentStatus.Cancelled)
                .ToList();

            var dto = _mapper.Map<IReadOnlyList<appointmentDtoforPatient>>(upcomingAppointments);
            return Result.Success(dto).ToActionResult();
        }

        [HttpGet("{id}/recent-notifications")]
        public async Task<IActionResult> GetRecentNotifications(int id, [FromQuery] int days = 7)
        {
            if (days <= 0 || days > 30)
                return Result.Failure<IReadOnlyList<RecentNotificationDto>>(new Error(400, "Invalid days range")).ToActionResult();

            var spec = new GetPatientWithNotificationsSpec(id, days);
            var patient = await _unitOfWork.Repository<Patient, int>().GetEntityWithSpecAsync(spec);
            if (patient == null)
                return Result.Failure<IReadOnlyList<RecentNotificationDto>>(new Error(404, "Patient not found")).ToActionResult();

            var dto = _mapper.Map<IReadOnlyList<RecentNotificationDto>>(patient.Notifications);
            return Result.Success(dto).ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivatePatient(int id)
        {
            var patient = await _unitOfWork.Repository<Patient, int>().GetByIdAsync(id);
            if (patient == null)
                return Result.Failure(new Error(404, "Patient not found")).ToActionResult();

            if (!patient.ISActive)
                return Result.Failure(new Error(400, "Patient is already inactive")).ToActionResult();

            patient.ISActive = false;
            _unitOfWork.Repository<Patient, int>().Update(patient);
            await _unitOfWork.CompleteAsync();

            return Result.Success().ToActionResult();
        }
    }
}
