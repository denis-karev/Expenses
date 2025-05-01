namespace Expenses.Api.Common;

public sealed class InvokeResult<T>
{
    public T? Result { get; }
    public Error? Error { get; }

    public Boolean IsSuccess => Error is null;
    public T CheckedResult => Result ?? throw new InvalidOperationException("Result is null");
    public Error CheckedError => Error ?? throw new InvalidOperationException("Error is null");
    
    private InvokeResult(T? result, Error? error) => (Result, Error) = (result, error);
    
    public static InvokeResult<T> CreateSuccess(T result) => new(result, null);
    public static InvokeResult<T> CreateError(Error error) => new(default, error);
    public static InvokeResult<T> CreateError(EErrorType error, String message) => new(default, new Error(error, message));
}