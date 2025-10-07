namespace BlogAPI.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                var (status, title) = ex switch
                {
                    NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                    ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden"),
                    UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                    BadRequestException or ArgumentException  => (StatusCodes.Status400BadRequest, "Bad Request"),
                    ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
                    _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
                };

                context.Response.StatusCode = status;

                // RFC7807-style body (similar to ProblemDetails)
                await context.Response.WriteAsJsonAsync(new
                {
                    type = "https://httpstatuses.com/" + status,
                    status,
                    title,
                    detail = ex.Message,
                    traceId = context.TraceIdentifier
                });
            }
        }
    }
}
