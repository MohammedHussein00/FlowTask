namespace FlowTask.Api.GraphQL.Types;

public class Payload<T>
{
    public T? Data { get; init; }
    public ApiErrorType? Error { get; init; }
    public bool Success => Error is null;

    public static Payload<T> Ok(T data) => new() { Data = data };
    public static Payload<T> Fail(ApiErrorType error) => new() { Error = error };
}

public class Payload
{
    public ApiErrorType? Error { get; init; }
    public bool Success => Error is null;

    public static Payload Ok() => new();
    public static Payload Fail(ApiErrorType error) => new() { Error = error };
}