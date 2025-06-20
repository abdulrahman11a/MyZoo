namespace Clinic.APIS.DTOS.Location_Dto
{
    
        public class CreateLocationDto
        {
            public string Name { get; set; } = null!;
            public string PhoneNumber { get; set; } = null!;
            public string Address { get; set; } = null!;
            public string Description { get; set; } = null!;

            public bool IsActive { get; set; } = true; //if locatonopen


        }
    

}