namespace TestLocalization.Helpers.Routing;

public class LanguageRouteConstraint(IOptions<SupportedAppLanguages> supportedAppLanguages) : IRouteConstraint
{
    public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        if (!values.ContainsKey("lang")) return false;

        // check for a match 
        var lang = values["lang"].ToString();
        return supportedAppLanguages.Value.Dict.Values.Any(langInApp => lang == langInApp.Icc);

    }
}
