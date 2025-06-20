namespace Clinic.Core.Entities
{
    public class Department: BaseEntity<int>
    {
        #region Properties
        public string DeptName { get; set; } = null!;
        public string Description { get; set; } = null!;

        #endregion

        #region Relationships
        public ICollection<Vet> Vets { get; set; } = new List<Vet>();
        #endregion

    }
}
