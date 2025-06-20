namespace Clinic.APIS.DTOS.VetDtos
{
    public class VetDto
    {

        #region Properties
            public int Id { get; set; } 
           public string Name { get; set; }
            public int Age { get; set; }
            public string email { get; set; } = null!;
            public DateTime DateOfGraduation { get; set; }
            #endregion

       #region Relationships 
            public string Department { get; set; }
            #endregion
        }
    }
