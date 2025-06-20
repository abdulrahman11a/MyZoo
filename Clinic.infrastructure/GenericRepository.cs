namespace Clinic.infrastructure
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext=dbContext;
        }
        #region CURD
        public async Task AddAsync(TEntity entity)
    => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void Update(TEntity entity)
    => _dbContext.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity)
            => _dbContext.Set<TEntity>().Remove(entity);




        #endregion

        #region Exists Item

        public async Task<bool> ExistsAsync(int id)
            => await _dbContext.Set<TEntity>().AnyAsync(e => e.Id!.Equals(id));

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
            => await _dbContext.Set<TEntity>().AnyAsync(predicate);

        #endregion

        #region  Without Specification

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(bool withNoTracking = true)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            if (withNoTracking) query = query.AsNoTracking();

            return await query.ToListAsync();
        }


        public async Task<TEntity?> GetByIdAsync(Tkey id)
            => await _dbContext.Set<TEntity>().FindAsync(id);
  

        #endregion

        #region Specifications
        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, Tkey> spec, bool withNoTracking = true)
        {
            var query = ApplySpecification(spec);
            if (withNoTracking)query = query.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetEntityWithSpecAsync(ISpecifications<TEntity, Tkey> spec)
        
          => await ApplySpecification(spec).AsNoTracking().FirstOrDefaultAsync();


        public async Task<int> GetCountAsync(ISpecifications<TEntity, Tkey> spec)
       
          =>await ApplySpecification(spec).AsNoTracking().CountAsync();
       



        private IQueryable<TEntity> ApplySpecification(ISpecifications<TEntity, Tkey> spec)
          => SpecificationsEvaluator<TEntity, Tkey>.ApplySpecification(_dbContext.Set<TEntity>(), spec);




        #endregion
    }
}
