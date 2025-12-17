using Amazon.Runtime;
using Amazon.S3;
using Droits.Clients;
using Droits.Data;
using Droits.Data.Mappers.Submission;
using Droits.Data.Mappers.Imports;
using Droits.Data.Mappers.Portal;
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
using Microsoft.AspNetCore.Server.Kestrel.Core;
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
var databaseOptions = builder.Configuration.GetSection("Database").Get<DatabaseOptions>();

var envDbHost = Environment.GetEnvironmentVariable("DB_HOST");
if ( !string.IsNullOrEmpty(envDbHost) && databaseOptions != null )
{
    databaseOptions.Host = envDbHost;
}

builder.Services.AddDbContext<DroitsContext>(opt =>
{
    var useInMemoryDb = false; //builder.Environment.IsDevelopment()
    if (useInMemoryDb)
    {
        opt.UseInMemoryDatabase(databaseOptions?.Database ?? "Droits");
    }
    else
    {
        opt.UseNpgsql(databaseOptions?.ConnectionString);
    }
});

// Session and HealthChecks
builder.Services.AddSession();
builder.Services.AddHealthChecks();

// Dependency Injections
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IMigrationService, MigrationService>();


builder.Services.AddScoped<ITokenValidationService, TokenValidationService>();

builder.Services.AddScoped<IGovNotifyClient, GovNotifyClient>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddScoped<ILetterRepository, LetterRepository>();
builder.Services.AddScoped<ILetterService, LetterService>();

builder.Services.AddScoped<IGovNotifyClient, GovNotifyClient>();

builder.Services.AddScoped<ICloudStorageClient, CloudStorageClient>();

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
builder.Services.AddScoped<IAzureBlobClient, AzureBlobClient>();

builder.Services.AddScoped<IDroitFileRepository, DroitFileRepository>();
builder.Services.AddScoped<IDroitFileService, DroitFileService>();

// Mappers
builder.Services.AddAutoMapper(typeof(DroitMappingProfile),typeof(SalvorMappingProfile),typeof(WreckMaterialMappingProfile),typeof(PowerAppsWreckMappingProfile), typeof(PowerAppsContactMappingProfile), typeof(PowerAppsDroitReportMappingProfile), typeof(PowerAppsWreckMaterialMappingProfile), typeof(PowerAppsNoteMappingProfile), typeof(PowerAppsUserMappingProfile), typeof(WebappSalvorInfoMappingProfile), typeof(WebappSalvorInfoDroitMappingProfile),typeof(WebappSalvorInfoWreckMaterialMappingProfile), typeof(WreckMaterialRowDtoMappingProfile), typeof(AccessDroitMappingProfile), typeof(AccessSalvorMappingProfile),typeof(AccessWreckMaterialMappingProfile));

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

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue; // Set the maximum size here as required
});


// ** APP BUILD & MIDDLEWARE **

var app = builder.Build();

// Error handling
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


// Seeding the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DroitsContext>();

    dbContext.Database.EnsureCreated();

    var shouldSeedDatabase = builder.Environment.IsDevelopment() && false;

    if (shouldSeedDatabase)
    {
        DatabaseSeeder.SeedData(dbContext);
    }
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
