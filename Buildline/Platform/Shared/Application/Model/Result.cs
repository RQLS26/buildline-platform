namespace Buildline.Platform.Shared.Application.Model;

/// <summary>
///     Represents the outcome of an application operation that returns a value.
/// </summary>
/// <typeparam name="T">Type of the value returned when the operation succeeds.</typeparam>
/// <remarks>
///     Result objects keep application services from throwing business exceptions. Bounded contexts
///     return success or typed enum errors, and REST assemblers translate those outcomes to HTTP.
/// </remarks>
public class Result<T>
{
    /// <summary>
    ///     Initializes a result instance.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation succeeded.</param>
    /// <param name="value">Value returned by a successful operation.</param>
    /// <param name="message">Localized or user-facing message associated with the result.</param>
    /// <param name="error">Typed bounded-context error associated with a failed operation.</param>
    protected Result(bool isSuccess, T? value, string message, Enum? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Message = message;
        Error = error;
    }

    /// <summary>
    ///     Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    ///     Gets a value indicating whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    ///     Gets the returned value when the operation succeeds.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    ///     Gets the message associated with the result.
    /// </summary>
    public string Message { get; }

    /// <summary>
    ///     Gets the typed bounded-context error when the operation fails.
    /// </summary>
    public Enum? Error { get; }

    /// <summary>
    ///     Creates a successful result with a value.
    /// </summary>
    /// <param name="value">Value produced by the operation.</param>
    /// <returns>A successful result containing the value.</returns>
    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, string.Empty, null);
    }

    /// <summary>
    ///     Creates a failed result with a typed error and message.
    /// </summary>
    /// <param name="error">Bounded-context error that describes the failure.</param>
    /// <param name="message">Localized or user-facing detail message.</param>
    /// <returns>A failed result.</returns>
    public static Result<T> Failure(Enum error, string message)
    {
        return new Result<T>(false, default, message, error);
    }
}

/// <summary>
///     Represents the outcome of an application operation that does not return a value.
/// </summary>
public class Result : Result<object>
{
    /// <summary>
    ///     Initializes a value-less result instance.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation succeeded.</param>
    /// <param name="message">Localized or user-facing message associated with the result.</param>
    /// <param name="error">Typed bounded-context error associated with a failed operation.</param>
    private Result(bool isSuccess, string message, Enum? error) : base(isSuccess, null, message, error)
    {
    }

    /// <summary>
    ///     Creates a successful value-less result.
    /// </summary>
    /// <returns>A successful result.</returns>
    public static Result Success()
    {
        return new Result(true, string.Empty, null);
    }

    /// <summary>
    ///     Creates a failed value-less result with a typed error and message.
    /// </summary>
    /// <param name="error">Bounded-context error that describes the failure.</param>
    /// <param name="message">Localized or user-facing detail message.</param>
    /// <returns>A failed result.</returns>
    public new static Result Failure(Enum error, string message)
    {
        return new Result(false, message, error);
    }
}
