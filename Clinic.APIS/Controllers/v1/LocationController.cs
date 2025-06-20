namespace Clinic.APIS.Controllers.v1
{
    [ApiVersion("1.0")]
    public class LocationController(IUnitOfWork _unitOfWork, IMapper _mapper) : ApiBaseController
    {
        [HttpGet("GetLocationsByVet/{vetId}")]
        public async Task<IActionResult> GetLocationsByVet(int vetId)
        {
            var spec = new LocationByVetSpecification(vetId);
            var locations = await _unitOfWork.Repository<Location, int>().GetAllWithSpecAsync(spec);

            if (locations == null || locations.Count == 0)
                return Result.Failure<IReadOnlyList<LocationDto>>(new Error(404, $"No locations found for vet ID {vetId}"))
                             .ToActionResult();

            var locationsDto = _mapper.Map<IReadOnlyList<LocationDto>>(locations);
            return Result.Success(locationsDto).ToActionResult();
        }

        [Cached(6000)]
        [HttpGet("GetLocations")]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _unitOfWork.Repository<Location, int>().GetAllAsync();
            var dto = _mapper.Map<IReadOnlyList<LocationOnlyDto>>(locations);
            return Result.Success(dto).ToActionResult();
        }

        [HttpGet("SearchLocationsByName")]
        public async Task<IActionResult> SearchLocationsByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
                return Result.Failure<IReadOnlyList<LocationDto>>(new Error(400, "Search query is required."))
                             .ToActionResult();

            var spec = new LocationSearchByNameSpecification(name);
            var locations = await _unitOfWork.Repository<Location, int>().GetAllWithSpecAsync(spec);
            var dto = _mapper.Map<IReadOnlyList<LocationDto>>(locations);

            return Result.Success(dto).ToActionResult();
        }

        [HttpPatch("UpdateAddress/{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] string newAddress)
        {
            if (string.IsNullOrWhiteSpace(newAddress))
                return Result.Failure<LocationDto>(new Error(400, "Address cannot be empty."))
                             .ToActionResult();

            var location = await _unitOfWork.Repository<Location, int>().GetByIdAsync(id);
            if (location == null)
                return Result.Failure<LocationDto>(new Error(404, $"Location with ID {id} not found."))
                             .ToActionResult();

            location.Address = newAddress;
            _unitOfWork.Repository<Location, int>().Update(location);
            await _unitOfWork.CompleteAsync();

            var updated = _mapper.Map<LocationDto>(location);
            return Result.Success(updated).ToActionResult();
        }

        [HttpPatch("UpdatePhoneNumber/{id}")]
        public async Task<IActionResult> UpdatePhoneNumber(int id, [FromBody] string newPhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(newPhoneNumber))
                return Result.Failure<LocationDto>(new Error(400, "Phone number cannot be empty."))
                             .ToActionResult();

            var location = await _unitOfWork.Repository<Location, int>().GetByIdAsync(id);
            if (location == null)
                return Result.Failure<LocationDto>(new Error(404, $"Location with ID {id} not found."))
                             .ToActionResult();

            location.PhoneNumber = newPhoneNumber;
            _unitOfWork.Repository<Location, int>().Update(location);
            await _unitOfWork.CompleteAsync();

            var updated = _mapper.Map<LocationDto>(location);
            return Result.Success(updated).ToActionResult();
        }

        [HttpPost("AddLocation")]
        public async Task<IActionResult> AddLocation([FromBody] CreateLocationDto dto)
        {
            if (dto == null)
                return Result.Failure<LocationDto>(new Error(400, "Location data must be provided."))
                             .ToActionResult();

            var location = _mapper.Map<Location>(dto);
            await _unitOfWork.Repository<Location, int>().AddAsync(location);
            await _unitOfWork.CompleteAsync();

            var locationDto = _mapper.Map<LocationDto>(location);
            return Result.Success(locationDto).ToActionResult();
        }

        [HttpDelete("DeleteLocation/{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _unitOfWork.Repository<Location, int>().GetByIdAsync(id);
            if (location == null)
                return Result.Failure(new Error(404, $"Location with ID {id} not found."))
                             .ToActionResult();

            _unitOfWork.Repository<Location, int>().Delete(location);
            await _unitOfWork.CompleteAsync();

            return Result.Success($"Location with ID {id} deleted successfully.")
                         .ToActionResult();
        }
    }
}
