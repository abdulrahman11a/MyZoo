namespace Clinic.infrastructure
{
    public static class SpecificationsEvaluator<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
       public static IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, Tkey> spec )
        {
            var Query = inputQuery;
            #region  Apply Criteria
            if (spec.Criteria is not null) Query= Query.Where(spec.Criteria);
            #endregion
            
            #region Apply Order
            if (spec.OrderBy != null)
            {
                Query=Query.OrderBy(spec.OrderBy);

            }
            else if (spec.OrderByDesc != null)
            {
                Query = Query.OrderByDescending(spec.OrderByDesc);

            }
            #endregion


            #region  Apply Includes
            if (spec.Includes.Count>0) Query = spec.Includes.Aggregate(Query, (current, include) => current.Include(include));


            if (spec.NavigationIncludes.Count > 0)
            {
                Query = spec.NavigationIncludes.Aggregate(Query, (current, include) => current.Include(include));
            }
            #endregion

            #region Pagination
            if (spec.IsPaginationEnabled)
                Query = Query.Skip(spec.Skip).Take(spec.Take); 

            #endregion
            return Query;
        }



    }
}
