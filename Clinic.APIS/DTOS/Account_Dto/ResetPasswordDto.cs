namespace Clinic.APIS.DTOS.Account_Dto
{
    public class ResetPasswordDto
    {
       public string NewPassword { get; set; } = null!;
        public string OldPassword { get; set; }
        public string Email { get; set; }
    }
}
