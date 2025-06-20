namespace Clinic.APIS.Mappings.Profiles
{
    public class ProfileVet : Profile
    {
        public ProfileVet()
        {
            #region ProfileVet
            CreateMap<Vet, VetDto>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department != null ? src.Department.DeptName : string.Empty));

            CreateMap<Vet, VetWithAppointmentsAndPatientsAndLocatioDto>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department != null ? src.Department.DeptName : string.Empty));

            CreateMap<Vet, GetVetsWithScheduleTimesDto>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department != null ? src.Department.DeptName : string.Empty));

            CreateMap<VetCreateDto, Vet>();
            CreateMap<VetUpdateDto, Vet>();

            CreateMap<Vet, GetVetsWithPrescriptionsDto>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department != null ? src.Department.DeptName : string.Empty))
                .ForMember(dest => dest.Prescriptions, opt => opt.MapFrom(src => src.Prescriptions));

            CreateMap<Vet, GetVetsWithNotificationsDto>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department != null ? src.Department.DeptName : string.Empty));
            #endregion
        }
    }
}
