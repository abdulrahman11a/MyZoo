namespace Clinic.Core.Services.Contract
{
    public interface IResponseCacheService
    {


        Task CacheResponseAsync(String CacheKey, object ResponseValue, TimeSpan TimeToLive);
        Task<string?> GetCachedResponseAsync(string CacheKey);

        
    }
}
