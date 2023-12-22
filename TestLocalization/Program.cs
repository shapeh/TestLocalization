var builder = WebApplication.CreateBuilder(args);

// enable resx localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// add ConstraintMap (not working).
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint)); // set constraint map for lang
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = false;
});


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // get all languages supported by app
    var supportedAppLanguages = builder.Configuration.GetSection("SupportedAppLanguages").Get<SupportedAppLanguages>();
    var supportedCultures = supportedAppLanguages.Dict.Values.Select(langInApp => new CultureInfo(langInApp.Culture)).ToList();

    options.DefaultRequestCulture = new RequestCulture(culture: "en-us", uiCulture: "en-us");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.FallBackToParentCultures = true;

    options.RequestCultureProviders.Remove(typeof(AcceptLanguageHeaderRequestCultureProvider)); // remove AcceptLanguageHeaderRequestCultureProvider
    options.RequestCultureProviders.Insert(0, new CustomRouteDataRequestCultureProvider() { Options = options, SupportedAppLanguages = supportedAppLanguages });
});

builder.Services.AddRazorPages(options =>
{
    // decorate all page routes with {lang} e.g. @page "/{lang}..."
    options.Conventions.Add(new CultureTemplatePageRouteModelConvention());
}).AddViewLocalization();


var app = builder.Build();

// handle baseUrl GET, i.e. request "/" 
app.MapGet("/", (context) =>
{
    context.Response.Redirect("/us");
    return Task.CompletedTask;
}).ShortCircuit(statusCode: StatusCodes.Status301MovedPermanently);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
