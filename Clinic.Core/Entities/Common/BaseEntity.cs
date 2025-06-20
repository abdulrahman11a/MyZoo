namespace Clinic.Core.Entities.Common
{
    public class BaseEntity<Tkey>
    {
        public Tkey Id { get; set; }
    }
}