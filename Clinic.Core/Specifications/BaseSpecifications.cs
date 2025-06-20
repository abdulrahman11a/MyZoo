namespace Clinic.Core.Specifications
{
    public abstract class BaseSpecifications<TEntity, Tkey> : ISpecifications<TEntity, Tkey>
       where TEntity : BaseEntity<Tkey>
    {
        #region properties
        public Expression<Func<TEntity, bool>>?Criteria { get; private set; }

        public List<Expression<Func<TEntity, object>>> Includes { get; } = new();
        public List<string> NavigationIncludes { get; } = new();
        #region explain NavigationIncludes way string 
        //        Includes List<Expression<Func<TEntity, object>>>	Normal includes
        //NavigationIncludes List<string>	Deep/nested includes
        #endregion
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        public Expression<Func<TEntity, object>> OrderByDesc { get; private set; }
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPaginationEnabled { get; set; }

        #endregion


        #region Set Criteria
        protected BaseSpecifications(Expression<Func<TEntity, bool>>?criteria)
        {
            Criteria = criteria;
        }
        #endregion

        #region AddInclude
        public void AddInclude(Expression<Func<TEntity, object>> expression)
        {
            Includes.Add(expression);
        }

        public void AddNavigationInclude(string navigationPath)
        {
            NavigationIncludes.Add(navigationPath);
        }

        #endregion

        #region sorting 
        public void AddOrderByAscending(Expression<Func<TEntity, object>> expression) => OrderBy=expression;
        public void AddOrderByDescending(Expression<Func<TEntity, object>> expression) => OrderByDesc=expression;

        #endregion

        #region Pagination
        protected void ApplyPagination(int skip, int take)
        {
          
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
        #endregion
    }

}

//Find cant  use wher use include is performance loss 
//Because use specification patter+ tow property cirteria  chose Func whay not chose bradct
//becuse whaer tack something  type Func  not bradict 