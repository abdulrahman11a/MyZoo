namespace Clinic.APIS.Mappings.Profiles
{
    public class ProfileScheduleTime:Profile
    {
       public ProfileScheduleTime()
        {
        #region ProfileScheduleTime
        CreateMap<ScheduleTime, ScheduleTimeDto>()
                .ForMember(dest => dest.VetName,
                opt => opt.MapFrom(src => src.Vet != null ? src.Vet.Name : string.Empty))
                .ForMember(dest => dest.LocationName,
                opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : string.Empty))
                .ForMember(dest => dest.StartTime,
                 opt => opt.MapFrom(src => src.StartTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.EndTime,
                opt => opt.MapFrom(src => src.EndTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.ScheduleGroup,
                 opt => opt.MapFrom(src => ScheduleTime_Config.GetEnumMemberValue(src.ScheduleGroup)));

            CreateMap<CreateScheduleTimeRequestDto, ScheduleTime>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Vet, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore());

        CreateMap<UpdateScheduleTimeRequestDto, ScheduleTime>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Vet, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            #endregion
        }
            
    }
}
