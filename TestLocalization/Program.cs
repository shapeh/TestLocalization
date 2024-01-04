var builder = WebApplication.CreateBuilder(args);

// enable resx localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// get all languages supported by app
var supportedAppLanguages = builder.Configuration.GetSection("SupportedAppLanguages").Get<SupportedAppLanguages>();
var supportedCultures = supportedAppLanguages.Dict.Values.Select(langInApp => new CultureInfo(langInApp.Culture)).ToList();

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = false;
    options.ConstraintMap.Add("cultureConstraint", typeof(CultureConstraint));
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(culture: "en-us", uiCulture: "en-us");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.FallBackToParentCultures = true;

    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Insert(0, new CustomRouteDataRequestCultureProvider() { Options = options, SupportedAppLanguages = supportedAppLanguages });
});

builder.Services.AddRazorPages(options =>
{
    // decorate all page routes with {lang} e.g. @page "/{lang}..."
    options.Conventions.Add(new CultureTemplatePageRouteModelConvention());
}).AddViewLocalization();

builder.Services.Configure<SupportedAppLanguages>(builder.Configuration.GetSection("AppLanguages"));

var app = builder.Build();
app.UseHttpsRedirection();

app.UseStaticFiles();

// handle baseUrl GET, i.e. request "/" 
app.MapGet("/", context =>
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

app.UseRouting();
app.UseRequestLocalization();

app.UseAuthorization();

//app.UseMiddleware<RouteConstraintMiddleware>(supportedAppLanguages);

app.MapRazorPages();
app.Run();
