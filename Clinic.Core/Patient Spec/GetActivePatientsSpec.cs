namespace Clinic.Core.Patient_Spec
{
    public class GetActivePatientsSpec : BaseSpecifications<Patient, int>
    {
        public  GetActivePatientsSpec():base(p=>p.ISActive==true)
        {



        }



    }
}