using Template.Contract.Errors;

namespace Template.Contract.Exceptions
{
    public class CustomValidationError : Exception
    {
        public List<ValidationError> ValidationErrors { get; set; }
        public CustomValidationError(List<ValidationError> validationErrors)
        {
            ValidationErrors = validationErrors;
        }
    }
}
