namespace Clinic.APIS.Mappings.Profiles
{
    public class ProfilePrescription : Profile
    {
        public ProfilePrescription()
        {
            #region Prescription → PrescriptionDto
            CreateMap<Prescription, PrescriptionDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : string.Empty))
                .ForMember(dest => dest.VetName,     opt => opt.MapFrom(src => src.Vet != null ? src.Vet.Name : string.Empty));
            #endregion

            #region CreatePrescriptionDTO → Prescription
            CreateMap<CreatePrescriptionDTO, Prescription>();
            #endregion

            #region UpdatePrescriptionDTO → Prescription
            CreateMap<UpdatePrescriptionDTO, Prescription>();
            #endregion
        }
    }
}
