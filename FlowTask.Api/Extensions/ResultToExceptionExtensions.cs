namespace FlowTask.Api.Extensions;

using FlowTask.Application.Exceptions;
using FlowTask.Shared.Results;

/// <summary>
/// Maps a ResultError (returned by handlers using Result&lt;T&gt;) back into the
/// matching AppException, so GraphQL mutations can `throw` it and let the
/// existing ErrorFilter/AppExceptionResponseFactory pipeline localize it —
/// same code path as handlers that throw AppException directly.
/// </summary>
public static class ResultToExceptionExtensions
{
    public static AppException ToAppException(this ResultError e) => e.Kind switch
    {
        ErrorKind.NotFound     => new NotFoundException(e.LocalizationKey, e.ResourceFile, e.Args),
        ErrorKind.Conflict     => e.Code == ErrorCodes.EmailAlreadyExists
                                    ? new EmailAlreadyExistsException((string)e.Args[0])
                                    : new ConflictException(e.LocalizationKey, e.ResourceFile, e.Args),
        ErrorKind.BadRequest   => new BadRequestException(e.LocalizationKey, e.ResourceFile, e.Args),
        ErrorKind.Unauthorized => new UnauthenticatedException(e.LocalizationKey, e.ResourceFile, e.Args),
        ErrorKind.Forbidden    => new ForbiddenException(e.LocalizationKey, e.ResourceFile, e.Args),
        ErrorKind.Validation   => new ValidationException(
                                      e.FieldErrors!.ToDictionary(kv => kv.Key, kv => kv.Value)),
        _                      => new BadRequestException(e.LocalizationKey, e.ResourceFile, e.Args),
    };
}