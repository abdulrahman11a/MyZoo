namespace Clinic.Core.Services.Contract
{
    public interface IAuthService
    {
        public Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);

    }
}
