namespace FlowTask.Api.Extensions;

using FlowTask.Api.ErrorHandling;
using FlowTask.Api.GraphQL.Types;
using FlowTask.Shared.Localization;
using FlowTask.Shared.Results;

public static class ResultPayloadExtensions
{
    public static Payload<TOut> ToPayload<TIn, TOut>(
        this Result<TIn> result, ILocalizationService loc, Func<TIn, TOut> map)
    {
        if (!result.IsSuccess)
        {
            var apiError = LocalizedErrorFactory.Build(result.Error!, loc);
            return Payload<TOut>.Fail(ApiErrorType.FromApiError(apiError));
        }

        return Payload<TOut>.Ok(map(result.Value!));
    }

    public static Payload ToPayload(this Result result, ILocalizationService loc)
    {
        if (!result.IsSuccess)
        {
            var apiError = LocalizedErrorFactory.Build(result.Error!, loc);
            return Payload.Fail(ApiErrorType.FromApiError(apiError));
        }
        return Payload.Ok();
    }
}