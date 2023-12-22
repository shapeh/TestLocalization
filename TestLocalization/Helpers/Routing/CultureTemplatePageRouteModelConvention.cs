namespace TestLocalization.Helpers.Routing;

public class CultureTemplatePageRouteModelConvention: IPageRouteModelConvention
{
    public void Apply(PageRouteModel model)
    {
        foreach (var selector in model.Selectors)
        {
            var template = selector.AttributeRouteModel.Template;

            // Skip the MicrosoftIdentity pages
            if (template.StartsWith("MicrosoftIdentity")) continue;

            // Prepend the /{lang}/ route value to allow for route-based localization
            selector.AttributeRouteModel.Template = AttributeRouteModel.CombineTemplates("{lang}", template);
        }
    }
}
