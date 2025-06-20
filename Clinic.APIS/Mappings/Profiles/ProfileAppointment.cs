namespace Clinic.APIS.Mappings.Profiles
{
    public class ProfileAppointment : Profile
    {
        public ProfileAppointment()
        {
            #region ProfileAppointment
            CreateMap<CreateAppointmentRequestDto, Appointment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.Vet, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore());

            CreateMap<Appointment, appointmentDtoforPatient>()
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet != null ? src.Vet.Name : string.Empty))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : string.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<UpdateAppointmentDto, Appointment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.Vet, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Appointment, appointmentDto>()
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet != null ? src.Vet.Name : string.Empty))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : string.Empty))
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : string.Empty));
            #endregion
        }
    }
}
