namespace Clinic.Core.Vet_Spec
{
     public class GetVetsByDepartmentSpec : BaseSpecifications<Vet, int>
     {
         public GetVetsByDepartmentSpec(int departmentId)
             : base(v => v.DepartmentId == departmentId)
         {
            AddInclude(v => v.Department);
        }
     }
}