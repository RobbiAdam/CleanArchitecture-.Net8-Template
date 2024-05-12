namespace Template.Contract.Errors
{
    public class ValidationError
    {
        public string PropertyName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
