
using Amazon.Runtime;
using Amazon.S3;
using Droits.Clients;
using Droits.Data;
using Droits.Data.Mappers;
using Droits.Data.Mappers.Powerapps;
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

// ** CONFIGURATION **

// HttpContext Access
builder.Services.AddHttpContextAccessor();

// Authentication and Authorization
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
    });

builder.Services.AddControllersWithViews(options =>
    {
        options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
        
        var adGroupId = builder.Configuration.GetSection("AzureAd:GroupId").Value ?? "";
        
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("groups", adGroupId)
            .Build();

        options.Filters.Add(new AuthorizeFilter(policy));
    })
    .AddRazorRuntimeCompilation().AddMicrosoftIdentityUI().AddSessionStateTempDataProvider();

var awsOptions = builder.Configuration.GetAWSOptions();

if ( !builder.Environment.IsDevelopment() )
{
    awsOptions.Credentials = new ECSTaskCredentials();
}

builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>();

builder.Services.AddRazorPages();

// Localization
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-GB");
});

// Database Context
builder.Services.AddDbContext<DroitsContext>(opt => opt.UseInMemoryDatabase("droits"));

// Session and HealthChecks
builder.Services.AddSession();
builder.Services.AddHealthChecks();

// Dependency Injections
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IApiService, ApiService>();

builder.Services.AddScoped<ITokenValidationService, TokenValidationService>();

builder.Services.AddScoped<IGovNotifyClient, GovNotifyClient>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddScoped<ILetterRepository, LetterRepository>();
builder.Services.AddScoped<ILetterService, LetterService>();

builder.Services.AddScoped<IGovNotifyClient, GovNotifyClient>();

builder.Services.AddScoped<IImageStorageClient, ImageStorageClient>();

builder.Services.AddScoped<IWreckMaterialRepository, WreckMaterialRepository>();
builder.Services.AddScoped<IWreckMaterialService, WreckMaterialService>();

builder.Services.AddScoped<IDroitRepository, DroitRepository>();
builder.Services.AddScoped<IDroitService, DroitService>();

builder.Services.AddScoped<IWreckRepository, WreckRepository>();
builder.Services.AddScoped<IWreckService, WreckService>();

builder.Services.AddScoped<ISalvorRepository, SalvorRepository>();
builder.Services.AddScoped<ISalvorService, SalvorService>();

builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageService, ImageService>();

// Mappers
builder.Services.AddAutoMapper(typeof(DroitMappingProfile),typeof(SalvorMappingProfile),typeof(WreckMaterialMappingProfile),typeof(PowerAppsWreckMappingProfile));

// GovUK Frontend
builder.Services.AddGovUkFrontend();

// Logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
});

// Forwarded Headers
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Cookie Policy
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

// ** APP BUILD & MIDDLEWARE **

var app = builder.Build();

// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Seeding the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DroitsContext>();
    DatabaseSeeder.SeedData(dbContext);
}

// Middleware Pipeline
app.UseForwardedHeaders();
app.UseRequestLocalization();
app.UseSession();
app.UseCookiePolicy();
app.UseStaticFiles();

app.MapHealthChecks("/healthz").AllowAnonymous();

app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthorization();

// Routing
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}"
    );
app.MapRazorPages();

app.Run();