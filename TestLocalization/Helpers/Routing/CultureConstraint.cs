namespace TestLocalization.Helpers.Routing;

public class CultureConstraint : IRouteConstraint
{
    public bool Match(
        HttpContext? httpContext, IRouter? route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.TryGetValue(routeKey, out var routeValue))
        {
            return false;
        }

        var supportedAppLanguages = httpContext.RequestServices.GetService<IConfiguration>().GetSection("SupportedAppLanguages").Get<SupportedAppLanguages>();
        var routeValueString = Convert.ToString(routeValue, CultureInfo.InvariantCulture);

        return supportedAppLanguages.Dict.Values.Any(langInApp => routeValueString == langInApp.Icc);
    }
}