namespace Clinic.Core.Patient_Spec
{
    public class PatientByNameSpec:BaseSpecifications<Patient,int>
    {
        public PatientByNameSpec(int Patientid):base(p=>p.Id==Patientid)
        {

        }


    }
}
