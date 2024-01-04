namespace TestLocalization.Helpers.Routing;

public class CultureTemplatePageRouteModelConvention: IPageRouteModelConvention
{
    public void Apply(PageRouteModel model)
    {
        foreach (var selector in model.Selectors)
        {
            var template = selector.AttributeRouteModel.Template;

            if (template.StartsWith("MicrosoftIdentity")) continue;  // Skip MicrosoftIdentity pages

            // Prepend {lang}/ to the page routes allow for route-based localization
            selector.AttributeRouteModel.Order = -1;
            selector.AttributeRouteModel.Template = AttributeRouteModel.CombineTemplates("{lang:cultureConstraint}", template);
        }
    }
}
