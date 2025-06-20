namespace Clinic.APIS.Mappings.Profiles
{
    public class ProfilePatient:Profile
    {
        public ProfilePatient()
        {
        #region ProfilePatient


        // Patient
        CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.ISActive));
            CreateMap<CreatePatientRequestDto, Patient>();
            CreateMap<UpdatePatientRequestDto, Patient>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Patient, PatientSummaryDto>()
                .ForMember(dest => dest.TotalAppointments, opt => opt.MapFrom(src => src.Appointments.Count))
                .ForMember(dest => dest.TotalPrescriptions, opt => opt.MapFrom(src => src.Prescriptions.Count))
                .ForMember(dest => dest.TotalNotifications, opt => opt.MapFrom(src => src.Notifications.Count))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.ISActive));
            CreateMap<Patient, PatientWithAppointmentsVetAndLocationDto>()
                .IncludeBase<Patient, PatientDto>();
            CreateMap<Patient, PatientWithNotificationsDto>()
                .IncludeBase<Patient, PatientDto>();
            CreateMap<Patient, PatientWithPrescriptionsDto>()
                .IncludeBase<Patient, PatientDto>();



            #endregion
        }
    }
}
