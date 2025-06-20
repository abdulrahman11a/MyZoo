namespace Clinic.Applacation
{
    public partial class AccountController
    {
        public class RegisterModel
        {
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
            public string DisplayName { get; set; } = null!;
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public string Street { get; set; } = null!;
            public string City { get; set; } = null!;
            public string Country { get; set; } = null!;
        }
    }
}