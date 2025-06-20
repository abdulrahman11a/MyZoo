namespace Clinic.APIS.Mappings.Profiles
{
    public class ProfileLocation:Profile
    {
        public ProfileLocation()
        {
            #region ProfileLocation
               CreateMap<Location, LocationDto>()
                .ForMember(dest => dest.Appointments, opt => opt.MapFrom(src => src.Appointments))
                .ForMember(dest => dest.ScheduleTimes, opt => opt.MapFrom(src => src.ScheduleTimes));

            CreateMap<CreateLocationDto, Location>();

            CreateMap<Location, LocationOnlyDto>();
            #endregion

        }
    }
}
