using Droits.Clients;
using Droits.Data;
using Droits.Middleware;
using Droits.ModelBinders;
using Droits.Repositories;
using Droits.Services;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
    });

builder.Services
    .AddControllersWithViews(options =>
    {
        options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
        
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        
        options.Filters.Add(new AuthorizeFilter(policy));
    })
    .AddRazorRuntimeCompilation().AddMicrosoftIdentityUI().AddSessionStateTempDataProvider();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-GB");
});

builder.Services.AddDbContext<DroitsContext>(opt => opt.UseInMemoryDatabase("droits"));

builder.Services.AddSession();
builder.Services.AddHealthChecks();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddScoped<ITokenValidationService, TokenValidationService>();

builder.Services.AddScoped<IGovNotifyClient, GovNotifyClient>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddScoped<ILetterRepository, LetterRepository>();
builder.Services.AddScoped<ILetterService, LetterService>();

builder.Services.AddScoped<IDroitRepository, DroitRepository>();
builder.Services.AddScoped<IWreckMaterialRepository, WreckMaterialRepository>();
builder.Services.AddScoped<IDroitService, DroitService>();

builder.Services.AddScoped<IWreckRepository, WreckRepository>();
builder.Services.AddScoped<IWreckService, WreckService>();

builder.Services.AddScoped<ISalvorRepository, SalvorRepository>();
builder.Services.AddScoped<ISalvorService, SalvorService>();


builder.Services.AddGovUkFrontend();

builder.Logging.AddConsole();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DroitsContext>();
    DatabaseSeeder.SeedData(dbContext);
}

app.UseRequestLocalization();
app.UseSession();
app.UseCookiePolicy();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthorization();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();
