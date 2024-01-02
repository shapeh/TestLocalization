namespace TestLocalization.Helpers.Routing;

public class CustomRouteDataRequestCultureProvider : RequestCultureProvider
{
    public SupportedAppLanguages SupportedAppLanguages;
    public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {

        var lang = (string)httpContext.GetRouteValue("lang");
        var urlCulture = httpContext.Request.Path.Value.Split('/')[1];

        string[] container = [lang, urlCulture];
        
        var culture = SupportedAppLanguages.Dict.Values.SingleOrDefault(langInApp => container.Contains(langInApp.Icc) );

        if (culture != null)
        {
            return Task.FromResult(new ProviderCultureResult(culture.Culture));
        }

        // if no match, return 404
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        return Task.FromResult(new ProviderCultureResult(Options.DefaultRequestCulture.Culture.TwoLetterISOLanguageName));
    }
}
