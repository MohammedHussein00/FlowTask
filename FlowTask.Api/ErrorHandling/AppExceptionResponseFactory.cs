namespace FlowTask.Api.ErrorHandling;

using FlowTask.Application.Exceptions;
using FlowTask.Shared.Localization;
using FlowTask.Shared.Results;

public static class AppExceptionResponseFactory
{
    public static ApiError Build(AppException ex, ILocalizationService loc)
        => LocalizedErrorFactory.Build(ex, loc);
}