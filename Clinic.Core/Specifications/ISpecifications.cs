namespace Clinic.Core.Specifications
{
    public interface ISpecifications<TEntity, Tkey>
      where TEntity : BaseEntity<Tkey>
    {
        Expression<Func<TEntity, bool>>?Criteria { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }
        public List<string> NavigationIncludes { get; }
        void AddInclude(Expression<Func<TEntity, object>> expression);

        public Expression<Func<TEntity, object>> OrderBy { get; }

        public Expression<Func<TEntity, object>> OrderByDesc { get; }
        public int Take { get;}
        public int Skip { get; }
        public bool IsPaginationEnabled { get; set; }
    }

}