namespace Clinic.infrastructure
{
    public class UnitOfWork (StoreDbContext dbContext) : IUnitOfWork
    {
        private readonly Hashtable _repositories = new();

        public IGenericRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            var entityType = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(entityType)) {
                var repository = new GenericRepository<TEntity, Tkey>(dbContext);
                _repositories.Add(entityType, repository);

            };
            return (IGenericRepository<TEntity, Tkey>)_repositories[entityType];
        }
        public async Task<int> CompleteAsync()

          =>  await dbContext.SaveChangesAsync();

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await dbContext.DisposeAsync();
        }

    }
}
