namespace TestLocalization.Helpers.Routing;

public class CustomRouteDataRequestCultureProvider : RequestCultureProvider
{
    public SupportedAppLanguages SupportedAppLanguages;
    public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var lang = (string)httpContext.Request.RouteValues["lang"];
        var urlCulture = httpContext.Request.Path.Value.Split('/')[1];

        var container = new[] {lang, urlCulture};
        
        var culture = SupportedAppLanguages.Dict.Values.SingleOrDefault(langInApp => container.Contains(langInApp.Icc) );

        if (culture != null)
        {
            return Task.FromResult(new ProviderCultureResult(culture.Culture));
        }

        // Use default culture
        return Task.FromResult(new ProviderCultureResult(Options.DefaultRequestCulture.Culture.TwoLetterISOLanguageName));
    }
}
