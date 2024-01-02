namespace TestLocalization.Helpers.Routing;

public class RouteConstraintMiddleware(RequestDelegate next, SupportedAppLanguages supportedAppLanguages) 
{
    public async Task Invoke(HttpContext context)
    {
        if (context.Response.StatusCode == StatusCodes.Status404NotFound) return;
        if (string.IsNullOrEmpty(context?.GetRouteValue("lang")?.ToString())) return;

        // check for a match 
        var lang = context.GetRouteValue("lang").ToString();
        var supported = supportedAppLanguages.Dict.Values.Any(langInApp => lang == langInApp.Icc);
       
        if (!supported)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        await next(context);
    }
}