namespace FlowTask.Api.GraphQL.Types;

using FlowTask.Shared.Results;

public class ApiErrorType
{
    public string Code { get; init; } = default!;
    public string Message { get; init; } = default!;
    public IReadOnlyList<ApiFieldErrorType> Errors { get; init; } = [];

    public static ApiErrorType FromApiError(ApiError e) => new()
    {
        Code = e.Code,
        Message = e.Message,
        Errors = e.Errors.Select(f => new ApiFieldErrorType
        {
            Field = f.Field,
            Message = f.Message,
            Code = f.Code,
        }).ToList(),
    };
}

public class ApiFieldErrorType
{
    public string Field { get; init; } = default!;
    public string Message { get; init; } = default!;
    public string Code { get; init; } = default!;
}