namespace FlowTask.Api.GraphQL.Filters;

using FlowTask.Api.ErrorHandling;
using FlowTask.Application.Exceptions;
using FlowTask.Shared.Localization;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AppErrorCodes = FlowTask.Shared.Results.ErrorCodes;

public sealed class ErrorFilter : IErrorFilter
{
    // ONLY IServiceProvider in the constructor — it's always resolvable,
    // regardless of which internal container HotChocolate uses to build this filter.
    private readonly IServiceProvider _rootServices;

    public ErrorFilter(IServiceProvider rootServices) => _rootServices = rootServices;

    public IError OnError(IError error)
    {
        var ex = error.Exception;
        if (ex is null) return error;

        if (ex is AggregateException { InnerExceptions.Count: 1 } agg)
            ex = agg.InnerExceptions[0];

        // Everything else resolved at runtime, inside a scope — never at
        // constructor time. This is what sidesteps HotChocolate's stricter
        // startup validation.
        using var scope = _rootServices.CreateScope();
        var sp = scope.ServiceProvider;
        var loc = sp.GetRequiredService<ILocalizationService>();

        if (ex is AppException appEx)
        {
            var apiError = AppExceptionResponseFactory.Build(appEx, loc);

            return error
                .WithMessage(apiError.Message)
                .WithException(null!)
                .SetExtension("code", apiError.Code)
                .SetExtension("errors", apiError.Errors);
        }

        var env = sp.GetRequiredService<IHostEnvironment>();
        var logger = sp.GetRequiredService<ILogger<ErrorFilter>>();
        logger.LogError(ex, "Unhandled GraphQL exception: {Message}", ex.Message);

        // Localized generic fallback — see below for why this beats a hardcoded string.
        var message = env.IsDevelopment()
            ? ex.Message
            : loc.Get("UnexpectedError", "Common");

        return error
            .WithMessage(message)
            .WithException(null!)
            .SetExtension("code", AppErrorCodes.Unexpected)
            .SetExtension("errors", Array.Empty<object>());
    }
}