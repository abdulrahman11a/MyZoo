namespace Clinic.Core.Department_Spec
{
    public class DepartmentWithVetsSpecification:BaseSpecifications<Department,int>
    {
        public DepartmentWithVetsSpecification():base(null)
        {
            AddInclude(d => d.Vets);

        }

    }
}
