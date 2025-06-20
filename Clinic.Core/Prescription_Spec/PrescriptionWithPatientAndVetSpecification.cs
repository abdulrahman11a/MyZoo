namespace Clinic.Core.Prescription_Spec
{
    public class PrescriptionWithPatientAndVetSpecification : BaseSpecifications<Prescription,int>
    {
        public PrescriptionWithPatientAndVetSpecification(int id):base(P=>P.Id==id)
        {
            AddInclude(p=>p.Patient);
            AddInclude(p=>p.Vet);   


        }

    }
}