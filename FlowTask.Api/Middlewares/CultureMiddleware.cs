// FlowTask.Api/Middlewares/CultureMiddleware.cs
namespace FlowTask.Api.Middlewares;

using System.Globalization;

public class CultureMiddleware
{
    private static readonly string[] Supported = ["en", "ar", "fr"];
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var lang = context.Request.Query["lang"].FirstOrDefault()
                   ?? context.Request.Headers["X-Language"].FirstOrDefault()
                   ?? context.Request.GetTypedHeaders().AcceptLanguage
                        ?.OrderByDescending(h => h.Quality ?? 1)
                        .Select(h => h.Value.ToString())
                        .FirstOrDefault(l => Supported.Contains(l.Split('-')[0]))
                   ?? "en";

        var culture = new CultureInfo(Supported.Contains(lang) ? lang : lang.Split('-')[0]);
        if (!Supported.Contains(culture.TwoLetterISOLanguageName))
            culture = new CultureInfo("en");

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await _next(context);
    }
}