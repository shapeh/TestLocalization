namespace TestLocalization.Helpers.Routing;

public class SupportedAppLanguages
{
    public Dictionary<string, Language> Dict { get; init; }
}

public class Language
{
    public string Icc { get; set; }

    public string Culture { get; set; }

}
