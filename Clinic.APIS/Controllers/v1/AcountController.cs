namespace Clinic.APIS.Controllers.v1
{
    [ApiVersion("1.0")]
    [EnableRateLimiting("AccountLimiter")]
    public class AccountController : ApiBaseController
    {
        #region Constructor & Dependencies
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly AppIdentityDBContext _identityContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AccountController(
            UserManager<AppUser> userManager,
            IConfiguration config,
            AppIdentityDBContext identityContext,
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _config = config;
            _identityContext = identityContext;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _emailService = emailService;
        } 
        #endregion
      
        #region Auth Endpoints

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(new { message = "Invalid email or password" });

            if (await _userManager.GetTwoFactorEnabledAsync(user))
                return await GenerateOTPFor2Factor(user);

            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var token = await _authService.CreateTokenAsync(user, _userManager);
            return Ok(new
            {
                token,
                user = new { user.Id, user.Email, user.DisplayName }
            });
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] ExternalLoginDto model)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken);
            if (payload == null || string.IsNullOrEmpty(payload.Email))
                return Unauthorized("Invalid Google token");

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
                return Unauthorized("User not found");

            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var token = await _authService.CreateTokenAsync(user, _userManager);

            return Ok(new
            {
                token,
                user = new { user.Id, user.Email, user.DisplayName }
            });
        }

        [HttpPost("register-Patient")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (await Check_email(model.Email))
                return BadRequest("Email already exists.");

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                DisplayName = model.DisplayName,
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(new { message = "Failed to create user", errors = result.Errors });

            await _userManager.AddToRoleAsync(user, "Patient");

            var address = new UserAddress
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Street = model.Street,
                City = model.City,
                Country = model.Country,
                AppUserId = user.Id
            };

            _identityContext.UserAddress.Add(address);
            await _identityContext.SaveChangesAsync();

            var patient = new Patient
            {
                Id = user.Id,
                Email = user.Email,
                FullName = $"{model.FirstName} {model.LastName}".Trim(),
                Address = $"{model.Street}, {model.City}, {model.Country}".Trim(),
                PhoneNumber = model.PhoneNumber ?? string.Empty
            };

            await _unitOfWork.Repository<Patient, int>().AddAsync(patient);
            await _unitOfWork.CompleteAsync();

            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var token = await _authService.CreateTokenAsync(user, _userManager);

            await _emailService.SendEmailAsync(
                user.Email,
                "Welcome to Clinic System",
                $"Hello {user.DisplayName}, your account has been created successfully."
            );

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            await _userManager.SetAuthenticationTokenAsync(user, "Email", "2FA", "true");

            return Ok(new
            {
                token,
                user = new { user.Id, user.Email, user.DisplayName }
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user == null) return Unauthorized();

            user.RefreshTokens.Clear();
            await _userManager.UpdateAsync(user);
            return Ok("Logged out successfully.");
        }

        [HttpPost("try-refresh-token")]
        public async Task<IActionResult> TryRefreshTokenAsync()
        {
            var refreshToken = Request.Headers["X-Refresh-Token"].FirstOrDefault();

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Refresh token is missing");

            var user = _userManager.Users.FirstOrDefault(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));
            if (user == null)
                return Unauthorized("Invalid refresh token");

            var storedToken = user.RefreshTokens.First(rt => rt.Token == refreshToken);
            if (storedToken.ExpireAt < DateTime.UtcNow)
                return Unauthorized("Refresh token expired");

            var newToken = await _authService.CreateTokenAsync(user, _userManager);
            return Ok(new { token = newToken });
        }


        #endregion

        #region Reset Password

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Result.Failure(new Error(400, "Email not found.")).ToActionResult();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded)
                return Result.Failure(new Error(400, "Password reset failed.")).ToActionResult();

            return Result.Success("Password has been reset.").ToActionResult();
        }

        #endregion

        #region Verify 2FA

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> VerifyTwoFactorCode([FromBody] Verify2FaDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Result.Failure(new Error(401, "User not found.")).ToActionResult();

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", model.Code);
            if (!isValid)
                return Result.Failure(new Error(401, "Invalid verification code.")).ToActionResult();

            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var token = await _authService.CreateTokenAsync(user, _userManager);

            var response = new
            {
                token,
                user = new { user.Id, user.Email, user.DisplayName }
            };

            return Result.Success(response).ToActionResult();
        }

        #endregion

        #region Get User Info

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUserAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result.Failure(new Error(401, "Unauthorized")).ToActionResult();

            var dto = new { user.Id, user.Email, user.DisplayName };
            return Result.Success(dto).ToActionResult();
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
           var exists=   await Check_email(email); 
            return Result.Success(exists).ToActionResult();
        }

        #endregion

        #region User Address

        [HttpGet("get-address")]
        public async Task<IActionResult> GetUserAddressAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure(new Error(401, "Unauthorized")).ToActionResult();

            var address = await _identityContext.UserAddress.FindAsync(user.Id);
            if (address == null)
                return Result.Failure(new Error(404, "Address not found")).ToActionResult();

            return Result.Success(address).ToActionResult();
        }

        [HttpPut("update-address")]
        public async Task<IActionResult> UpdateUserAddressAsync(UserAddress address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure(new Error(401, "Unauthorized")).ToActionResult();

            var currentAddress = await _identityContext.UserAddress.FindAsync(user.Id);
            if (currentAddress == null)
                return Result.Failure(new Error(404, "Address not found")).ToActionResult();

            currentAddress.FirstName = address.FirstName;
            currentAddress.LastName = address.LastName;
            currentAddress.Street = address.Street;
            currentAddress.City = address.City;
            currentAddress.Country = address.Country;

            await _identityContext.SaveChangesAsync();

            return Result.Success("Address updated successfully.").ToActionResult();
        }

        #endregion

        #region Helpers

        private RefreshToken GenerateRefreshToken() => new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            ExpireAt = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:DurationInMinutes"] ?? "30"))
        };
        private async Task<IActionResult> GenerateOTPFor2Factor(AppUser user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);

            if (!providers.Contains("Email"))
            {
                return Unauthorized(new AuthResponseDto
                {
                    ErrorMessage = "Invalid 2-Factor Provider."
                });
            }

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            await _emailService.SendEmailAsync(
                user.Email,
                "2FA Authentication Code",
                $"Your verification code is: <b>{token}</b>"
            );

            return Ok(new AuthResponseDto
            {
                Is2FactorRequired = true,
                Provider = "Email"
            });
        }
        private async Task<bool> Check_email(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        #endregion
    }
}
