namespace Clinic.APIS.Controllers.v1
{
    [ApiVersion("1.0")]
    public class DepartmentController(IUnitOfWork unitOfWork, IMapper mapper) : ApiBaseController
    {
        [HttpGet("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartmentsWithVets()
        {
            var spec = new DepartmentWithVetsSpecification();
            var departments = await unitOfWork.Repository<Department, int>().GetAllWithSpecAsync(spec);
            var departmentsDto = mapper.Map<List<DepartmentDto>>(departments);

            var result = Result.Success<IReadOnlyList<DepartmentDto>>(departmentsDto);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var department = await unitOfWork.Repository<Department, int>().GetByIdAsync(id);

            if (department == null)
                return Result.Failure<Department>(new Error(404, $"Department with ID {id} not found."))
                             .ToActionResult();

            return Result.Success(department).ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return Result.Failure(new Error(400, "Invalid department data.")).ToActionResult();
            }

            await unitOfWork.Repository<Department, int>().AddAsync(department);
            await unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);

        }
    }
}
