namespace Clinic.APIS.Controllers.v1
{
    [ApiVersion("1.0")]
    public class VetController(IUnitOfWork unitOfWork, IMapper mapper, IAppointmentsService appointmentsService) : ApiBaseController
    {
        [CachedAttribute(600)]
        [HttpGet("GetAllVet")]
        public async Task<IActionResult> GetAllVets()
        {
            var vetsSpec = new VetWithAppointmentsAndScheduleTimesAndPrescriptionNotificationSpecification();
            var vets = await unitOfWork.Repository<Vet, int>().GetAllWithSpecAsync(vetsSpec);
            var result = Result.Success(mapper.Map<IReadOnlyList<VetDto>>(vets));
            return result.ToActionResult();
        }

        [HttpGet("GetVetById/{id}")]
        public async Task<IActionResult> GetVetById(int id)
        {
            var vetsSpec = new VetWithAppointmentsAndScheduleTimesAndPrescriptionNotificationSpecification(id);
            var vet = await unitOfWork.Repository<Vet, int>().GetEntityWithSpecAsync(vetsSpec);
            var result = vet is null
                ? Result.Failure<VetDto>(new Error(404, "Vet Not Found"))
                : Result.Success(mapper.Map<VetDto>(vet));
            return result.ToActionResult();
        }

        [HttpGet("GetVetByName")]
        public async Task<IActionResult> GetVetByName([FromQuery] string name)
        {
            var vetSpec = new GetVetByNameSpec(name);
            var vet = await unitOfWork.Repository<Vet, int>().GetEntityWithSpecAsync(vetSpec);
            var result = vet is null
                ? Result.Failure<VetDto>(new Error(404, "Vet Not Found"))
                : Result.Success(mapper.Map<VetDto>(vet));
            return result.ToActionResult();
        }

        [HttpGet("VetWithAppointmentsAndPatientsAndLocatio")]
        public async Task<IActionResult> GetVetsWithAppointments()
        {
            var vetsSpec = new VetWithAppointmentsAndPatientsSpecAndLocatio();
            var vets = await unitOfWork.Repository<Vet, int>().GetAllWithSpecAsync(vetsSpec);
            var result = Result.Success(mapper.Map<IReadOnlyList<VetWithAppointmentsAndPatientsAndLocatioDto>>(vets));
            return result.ToActionResult();
        }

        [HttpGet("GetVetWithAppointmentsById/{id}")]
        public async Task<IActionResult> GetVetWithAppointmentsById(int id)
        {
            var vetSpec = new VetWithAppointmentsAndPatientsSpecAndLocatio(id);
            var vet = await unitOfWork.Repository<Vet, int>().GetEntityWithSpecAsync(vetSpec);
            var result = vet is null
                ? Result.Failure<VetWithAppointmentsAndPatientsAndLocatioDto>(new Error(404, "Vet Not Found"))
                : Result.Success(mapper.Map<VetWithAppointmentsAndPatientsAndLocatioDto>(vet));
            return result.ToActionResult();
        }

        [HttpGet("GetVetsWithScheduleTimes")]
        public async Task<IActionResult> GetVetsWithScheduleTimes()
        {
            var vetsSpec = new VetWithScheduleTimesSpecification();
            var vets = await unitOfWork.Repository<Vet, int>().GetAllWithSpecAsync(vetsSpec);
            var result = Result.Success(mapper.Map<IReadOnlyList<GetVetsWithScheduleTimesDto>>(vets));
            return result.ToActionResult();
        }

        [HttpGet("GetVetWithScheduleTimesById/{id}")]
        public async Task<IActionResult> GetVetWithScheduleTimesById(int id)
        {
            var vetSpec = new VetWithScheduleTimesSpecification(id);
            var vet = await unitOfWork.Repository<Vet, int>().GetEntityWithSpecAsync(vetSpec);
            var result = vet is null
                ? Result.Failure<GetVetsWithScheduleTimesDto>(new Error(404, "Vet Not Found"))
                : Result.Success(mapper.Map<GetVetsWithScheduleTimesDto>(vet));
            return result.ToActionResult();
        }

        [HttpGet("GetVetsWithPrescriptions")]
        public async Task<IActionResult> GetVetsWithPrescriptions()
        {
            var vetsSpec = new GetVetsWithPrescriptionsSpec();
            var vets = await unitOfWork.Repository<Vet, int>().GetAllWithSpecAsync(vetsSpec);
            var result = Result.Success(mapper.Map<IReadOnlyList<GetVetsWithPrescriptionsDto>>(vets));
            return result.ToActionResult();
        }

        [HttpGet("GetVetWithPrescriptionsById/{id}")]
        public async Task<IActionResult> GetVetWithPrescriptionsById(int id)
        {
            var vetSpec = new GetVetsWithPrescriptionsSpec(id);
            var vet = await unitOfWork.Repository<Vet, int>().GetEntityWithSpecAsync(vetSpec);
            var result = vet is null
                ? Result.Failure<GetVetsWithPrescriptionsDto>(new Error(404, "Vet Not Found"))
                : Result.Success(mapper.Map<GetVetsWithPrescriptionsDto>(vet));
            return result.ToActionResult();
        }

        [HttpGet("GetVetsWithNotifications")]
        public async Task<IActionResult> GetVetsWithNotifications()
        {
            var vetsSpec = new VetWithNotificationSpecification();
            var vets = await unitOfWork.Repository<Vet, int>().GetAllWithSpecAsync(vetsSpec);
            var result = Result.Success(mapper.Map<IReadOnlyList<GetVetsWithNotificationsDto>>(vets));
            return result.ToActionResult();
        }

        [HttpGet("GetVetWithNotificationsById/{id}")]
        public async Task<IActionResult> GetVetWithNotificationsById(int id)
        {
            var vetSpec = new VetWithNotificationSpecification(id);
            var vet = await unitOfWork.Repository<Vet, int>().GetEntityWithSpecAsync(vetSpec);
            var result = vet is null
                ? Result.Failure<VetDto>(new Error(404, "Vet Not Found"))
                : Result.Success(mapper.Map<VetDto>(vet));
            return result.ToActionResult();
        }

        [HttpGet("GetActiveVets")]
        public async Task<IActionResult> GetActiveVets()
        {
            var vetGetActiveVets = new GetActiveVetsSpec();
            var activeVets = await unitOfWork.Repository<Vet, int>().GetAllWithSpecAsync(vetGetActiveVets);
            var result = Result.Success(mapper.Map<IReadOnlyList<VetDto>>(activeVets));
            return result.ToActionResult();
        }

        [HttpGet("GetVetsByDepartment/{departmentId}")]
        public async Task<IActionResult> GetVetsByDepartment(int departmentId)
        {
            var vetsSpec = new GetVetsByDepartmentSpec(departmentId);
            var vets = await unitOfWork.Repository<Vet, int>().GetAllWithSpecAsync(vetsSpec);
            var result = Result.Success(mapper.Map<IReadOnlyList<VetDto>>(vets));
            return result.ToActionResult();
        }

        [HttpGet("CountVets")]
        public async Task<IActionResult> CountVets()
        {
            var vetGetActiveVets = new GetActiveVetsSpec();
            var count = await unitOfWork.Repository<Vet, int>().GetCountAsync(vetGetActiveVets);
            var result = Result.Success(count);
            return result.ToActionResult();
        }

        [HttpGet("SearchVetsByName")]
        public async Task<IActionResult> SearchVetsByName([FromQuery] string name)
        {
            var vetSpec = new SearchVetsByNameSpec(name);
            var vets = await unitOfWork.Repository<Vet, int>().GetAllWithSpecAsync(vetSpec);
            var result = Result.Success(mapper.Map<IReadOnlyList<VetDto>>(vets));
            return result.ToActionResult();
        }

        [HttpGet("vet-Pendingappointments/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsForVet(int doctorId)
        {
            var appointments = await appointmentsService.GetAllAppointmentsForVetAsync(doctorId);
            var result = (appointments == null || !appointments.Any()) ?
             Result.Failure<List<appointmentDto>>(new Error(404, "No appointments found for this vet.")) :
             Result.Success(mapper.Map<List<appointmentDto>>(appointments));
            return result.ToActionResult();
        }

        [HttpPost("AddVet")]
        public async Task<IActionResult> AddVet([FromBody] VetCreateDto vetCreateDto)
        {
            var vet = mapper.Map<Vet>(vetCreateDto);
            await unitOfWork.Repository<Vet, int>().AddAsync(vet);
            await unitOfWork.CompleteAsync();
            var result = Result.Success(mapper.Map<VetDto>(vet));
            return CreatedAtAction(nameof(GetVetById), new { id = vet.Id }, result.Value);
        }

        [HttpPut("UpdateVet/{id}")]
        public async Task<IActionResult> UpdateVet(int id, [FromBody] VetUpdateDto vetUpdateDto)
        {
            var vet = await unitOfWork.Repository<Vet, int>().GetByIdAsync(id);
            if (vet == null)
                return NotFound();
            mapper.Map(vetUpdateDto, vet);
            unitOfWork.Repository<Vet, int>().Update(vet);
            await unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("DeleteVet/{id}")]
        public async Task<IActionResult> DeleteVet(int id)
        {
            var vet = await unitOfWork.Repository<Vet, int>().GetByIdAsync(id);
            if (vet == null)
                return NotFound();
            unitOfWork.Repository<Vet, int>().Delete(vet);
            await unitOfWork.CompleteAsync();
            return NoContent();
        }

    }
}
