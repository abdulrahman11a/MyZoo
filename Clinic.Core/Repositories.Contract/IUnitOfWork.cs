namespace Clinic.Core.Repositories.Contract
{
    public interface IUnitOfWork: IAsyncDisposable, IDisposable
    {
        IGenericRepository<TEntity,Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>;

        Task<int> CompleteAsync();

    }
}