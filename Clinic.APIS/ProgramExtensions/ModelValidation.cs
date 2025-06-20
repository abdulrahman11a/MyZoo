namespace Clinic.APIS.ProgramExtensions
{
    public static class ModelValidation
    {
        public static IServiceCollection AddModelValidation(this IServiceCollection services)
        {
            #region Model Validation

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            #endregion

            return services;
        }
    }
}
