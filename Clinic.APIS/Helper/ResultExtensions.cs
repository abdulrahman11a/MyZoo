namespace Clinic.APIS.Helper
{
    public static class ResultExtensions
    {
        public static ActionResult ToProblem(this Result result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException("Cannot convert success result to a problem");

            var error = new ErrorResponse
            {
                StatusCode = result.Error?.StatusCode ?? 500,
                Message = result.Error?.Title ?? "An error occurred"
            };

            return new ObjectResult(error)
            {
                StatusCode = error.StatusCode
            };
        }
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            return result.IsSuccess
                ? new OkObjectResult(result.Value)
                : new ObjectResult(new ProblemDetails
                {
                    Status = result.Error?.StatusCode,
                    Title = result.Error?.Title
                })
                {
                    StatusCode = result.Error?.StatusCode
                };
        }

        public static IActionResult ToActionResult(this Result result)
        {
            return result.IsSuccess
                ? new OkObjectResult(result.SuccessMessage ?? "Operation completed successfully.")
                : new ObjectResult(new ProblemDetails
                {
                    Status = result.Error?.StatusCode,
                    Title = result.Error?.Title
                })
                {
                    StatusCode = result.Error?.StatusCode
                };
        }
    }
}
