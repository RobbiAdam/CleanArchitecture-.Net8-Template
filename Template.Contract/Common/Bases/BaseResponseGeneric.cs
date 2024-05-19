namespace Template.Contract.Common.Bases
{
    public class BaseResponseGeneric<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public IEnumerable<BaseError>? Errors { get; set; }

    }
}
