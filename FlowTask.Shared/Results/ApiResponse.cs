namespace FlowTask.Shared.Results;

/// <summary>
/// Single outer envelope for every API response — REST or GraphQL-adjacent
/// REST endpoints. On success, Data is populated. On failure, Error is
/// populated with the same rich shape ExceptionMiddleware already produces,
/// so the frontend has exactly one shape to parse regardless of failure
/// source (thrown AppException vs returned Result<T>).
/// </summary>
public sealed class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public ApiError? Error { get; init; }

    public static ApiResponse<T> Ok(T data) =>
        new() { Success = true, Data = data };

    public static ApiResponse<T> Fail(ApiError error) =>
        new() { Success = false, Error = error };
}