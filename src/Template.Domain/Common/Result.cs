using Template.Domain.Abstractions;

namespace Template.Domain.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; }
        public Error Error { get; }

        public static implicit operator Result<T>(T value) => new (value, true, Error.None);
        public static implicit operator Result<T>(Error error) => new( default, false, error);

        private Result(T? value, bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid Error", nameof(error));
            }

            Value = value;
            IsSuccess = isSuccess;
            Error = error;
        }
    }
}