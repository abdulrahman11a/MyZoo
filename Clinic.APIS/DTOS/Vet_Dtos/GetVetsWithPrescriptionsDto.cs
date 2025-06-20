namespace Clinic.APIS.DTOS.VetDtos
{
    public class GetVetsWithPrescriptionsDto
    {

        #region Properties
        public string Name { get; set; }

        #endregion


        #region Relationships 

        public ICollection<PrescriptionDto> Prescriptions { get; set; }
        
        public string Department { get; set; }
        #endregion
    }
}
