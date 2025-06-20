namespace Clinic.Core.Location_Spec
{
    public class SearchPatientsByNameSpec : BaseSpecifications<Patient, int>
    {
        public SearchPatientsByNameSpec(string name):base(p=>p.FullName.Contains(name))
        {



        }
    }
}