namespace Clinic.Core.Entities.Identity
{
    public  class AppUser:IdentityUser<int>
    {
       
        public string DisplayName { get; set; } = null!;
        public UserAddress Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public  ICollection<RefreshToken>? RefreshTokens { get; set; } = [];

    }
}
namespace Clinic.Core.Entities.Identity
{
    public class AppRole : IdentityRole<int>
    {
    }
}
