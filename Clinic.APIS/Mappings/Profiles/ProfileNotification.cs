namespace Clinic.APIS.Mappings.Profiles
{
    public class ProfileNotification:Profile
    {
        public ProfileNotification()
        {
        #region ProfileNotification
        CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet != null ? src.Vet.Name : string.Empty))
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : string.Empty));


     
            CreateMap<Notification, RecentNotificationDto>()
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet != null ? src.Vet.Name : string.Empty))
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : string.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.STAT));
            #endregion
        }
    }
}
