namespace Clinic.Applacation
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            // Claims
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Add roles to claims
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // JWT Key from configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"] ?? "DefaultSecretKey"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Expiry duration (using a fallback value if missing in the configuration)
            var tokenExpirationMinutes = Convert.ToDouble(_configuration["JWT:DurationInMinutes"] ?? "30");

            // Create token
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"] ?? "DefaultIssuer",
                audience: _configuration["JWT:Audience"] ?? "DefaultAudience",
                claims: authClaims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    

    }
}
