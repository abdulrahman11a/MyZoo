 namespace Clinic.APIS.DTOS.Account_Dto
{
     public  class AuthResponseDto
    {
        public bool Is2FactorRequired { get; set; }
        public string? Provider { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
