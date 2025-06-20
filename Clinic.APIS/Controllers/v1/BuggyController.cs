namespace Clinic.APIS.Controllers.v1
{
    [Route("api/[controller]")]
    public class BuggyController(StoreDbContext storeDbContext) : ApiBaseController
    {
        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            return Result.Failure(new Error(404, "Resource not found"))
                         .ToActionResult();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return Result.Failure(new Error(400, "Bad request"))
                         .ToActionResult();
        }

        [HttpGet("unauthorize")]
        public IActionResult GetUnauthorizeError()
        {
            return Result.Failure(new Error(401, "Unauthorized access"))
                         .ToActionResult();
        }

        // Error: Not Found or Unauthorized Endpoint
        // Used when a client tries to access a non-existing endpoint,
        // or attempts to access a protected endpoint without a valid token.
        [HttpGet("badrequest/{id:int}")]
        public IActionResult GetBadRequest(int id)
        {
            return Result.Failure(new Error(400, $"Invalid request with id: {id}"))
                         .ToActionResult();
        }

        [HttpGet("servererror")]
        public IActionResult GetServerError()
        {
            var Appointment = storeDbContext.Appointments.Find(100);

            if (Appointment == null)
                return Result.Failure(new Error(500, "Internal Server Error: Appointment not found."))
                             .ToActionResult();

            return Result.Success(Appointment.ToString())
                         .ToActionResult();
        }
    }
}
