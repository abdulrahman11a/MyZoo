namespace Clinic.Applacation
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _Database;

        public ResponseCacheService(IConnectionMultiplexer Database)
        {
            _Database=Database.GetDatabase();
        }

        public async Task CacheResponseAsync(string CacheKey, object ResponseValue, TimeSpan TimeToLive)
        {
            if (ResponseValue is null) return;
            var serilizerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }; 
            var serilizedResponse=JsonSerializer.Serialize(ResponseValue, serilizerOptions);    
           await _Database.StringSetAsync(CacheKey, serilizedResponse,TimeToLive);   
        }

        public async Task<string?> GetCachedResponseAsync(string CacheKey)
        {
           var CacheResponsData= await _Database.StringGetAsync(CacheKey);
            if (CacheResponsData.IsNullOrEmpty) return null;
            return CacheResponsData;    
        }
    }
}
