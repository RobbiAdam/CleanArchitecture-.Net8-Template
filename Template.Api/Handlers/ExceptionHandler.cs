using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Template.Contract.Exceptions;

namespace Template.Api.Handlers
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = CreateProblemDetails(exception);
            httpContext.Response.StatusCode =
                problemDetails.Status ?? StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

        private static ProblemDetails CreateProblemDetails(Exception exception)
        {
            return exception switch
            {
                NotFoundException ex => NotFoundProblemDetails(ex),
                CustomValidationError ex => ValidationErrorProblemDetails(ex),
                _ => InternalServerErrorProblemDetails(exception),
            };
        }

        private static ProblemDetails NotFoundProblemDetails(NotFoundException ex)
        {
            return CreateProblemDetails(
                StatusCodes.Status404NotFound,
                "Not Found",
                ex.Message,
                "https://tools.ietf.org/html/rfc7231#section-6.5.4");
        }

        private static ProblemDetails ValidationErrorProblemDetails(CustomValidationError ex)
        {
            var details = CreateProblemDetails(
                StatusCodes.Status400BadRequest,
                "Validation Error",
                "One or more validation errors occurred",
                "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1");

            details.Extensions["errors"] = ex.ValidationErrors;

            return details;
        }
        private static ProblemDetails InternalServerErrorProblemDetails(Exception ex)
        {
            return CreateProblemDetails(
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An error occured while processing your request",
                "https://tools.ietf.org/html/rfc7231#section-6.6.1");
        }
        private static ProblemDetails CreateProblemDetails
            (int status, string title, string detail, string type)
        {
            return new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Type = type
            };
        }
    }
}
