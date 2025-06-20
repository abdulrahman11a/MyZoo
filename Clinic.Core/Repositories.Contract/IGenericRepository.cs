namespace Clinic.Core.Repositories.Contract
{
    public interface IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync(bool withNoTracking = true);
        Task<TEntity?> GetByIdAsync(Tkey id);
        Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity,Tkey> spec, bool withNoTracking = true);
        Task<TEntity?> GetEntityWithSpecAsync(ISpecifications<TEntity,Tkey> spec);
        Task<int> GetCountAsync(ISpecifications<TEntity,Tkey> spec);
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
            Task<bool> ExistsAsync(int id);
            Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
           
        

    }
}