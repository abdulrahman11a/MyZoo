namespace Clinic.Core.ErrorHandling
{
    public class Error
    {
        public static readonly Error? None = new(200, string.Empty);

        public int StatusCode { get; set; }
        public string Title { get; set; }

        public Error(int statusCode, string? title = null)
        {
            StatusCode = statusCode;
            Title = title ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                #region 1xx - Informational
                100 => "Continue",
                101 => "Switching Protocols",
                102 => "Processing",
                103 => "Early Hints",
                #endregion

                #region 2xx - Success
                200 => "OK",
                201 => "Created",
                202 => "Accepted",
                203 => "Non-Authoritative Information",
                204 => "No Content",
                205 => "Reset Content",
                206 => "Partial Content",
                #endregion

                #region 3xx - Redirection
                300 => "Multiple Choices",
                301 => "Moved Permanently",
                302 => "Found",
                303 => "See Other",
                304 => "Not Modified",
                307 => "Temporary Redirect",
                308 => "Permanent Redirect",
                #endregion

                #region 4xx - Client Errors
                400 => "Bad Request",
                401 => "Unauthorized",
                402 => "Payment Required",
                403 => "Forbidden",
                404 => "Not Found",
                405 => "Method Not Allowed",
                406 => "Not Acceptable",
                407 => "Proxy Authentication Required",
                408 => "Request Timeout",
                409 => "Conflict",
                410 => "Gone",
                411 => "Length Required",
                412 => "Precondition Failed",
                413 => "Payload Too Large",
                414 => "URI Too Long",
                415 => "Unsupported Media Type",
                416 => "Range Not Satisfiable",
                417 => "Expectation Failed",
                418 => "I'm a teapot",
                422 => "Unprocessable Entity",
                426 => "Upgrade Required",
                429 => "Too Many Requests",
                #endregion

                #region 5xx - Server Errors
                500 => "Internal Server Error",
                501 => "Not Implemented",
                502 => "Bad Gateway",
                503 => "Service Unavailable",
                504 => "Gateway Timeout",
                505 => "HTTP Version Not Supported",
                507 => "Insufficient Storage",
                508 => "Loop Detected",
                510 => "Not Extended",
                #endregion

                _ => "Unexpected error occurred"
            };
        }


    }
}
