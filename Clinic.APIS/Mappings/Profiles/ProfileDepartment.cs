namespace Clinic.APIS.Mappings.Profiles
{
    public class ProfileDepartment : Profile
    {
        public ProfileDepartment()
        {
            #region ProfileDepartment
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.DeptName, opt => opt.MapFrom(src => src.DeptName ?? string.Empty));
            #endregion
        }
    }
}
