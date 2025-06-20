namespace Clinic.Core.Patient_Spec
{
    public class GetNonActivePatientsSpec : BaseSpecifications<Patient, int>
    {
       public GetNonActivePatientsSpec():base(p=>p.ISActive==false)
        {


        }

    }
}