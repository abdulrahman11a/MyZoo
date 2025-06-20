namespace Clinic.APIS.DTOS.Department
{
    public class DepartmentDto
    {
        public string DeptName { get; set; }
        public string Description { get; set; }
        public List<VetDto> Vets { get; set; }
    }

}

