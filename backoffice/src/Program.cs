using Droits.Clients;
using Droits.Data;
using Droits.Middleware;
using Droits.ModelBinders;
using Droits.Repositories;
using Droits.Services;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
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

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddScoped<ILetterRepository, LetterRepository>();
builder.Services.AddScoped<ILetterService, LetterService>();

builder.Services.AddScoped<IGovNotifyClient, GovNotifyClient>();

builder.Services.AddScoped<IDroitRepository, DroitRepository>();
builder.Services.AddScoped<IWreckMaterialRepository, WreckMaterialRepository>();
builder.Services.AddScoped<IDroitService, DroitService>();

builder.Services.AddScoped<IWreckRepository, WreckRepository>();
builder.Services.AddScoped<IWreckService, WreckService>();

builder.Services.AddScoped<ISalvorRepository, SalvorRepository>();
builder.Services.AddScoped<ISalvorService, SalvorService>();


builder.Services.AddScoped<ITokenValidationService, TokenValidationService>();


builder.Services.AddGovUkFrontend();

builder.Logging.AddConsole();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
});


var app = builder.Build();

app.MapHealthChecks("/healthz");

// Configure the HTTP request pipeline.
if ( !app.Environment.IsDevelopment() )
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using ( var scope = app.Services.CreateScope() )
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DroitsContext>();
    DatabaseSeeder.SeedData(dbContext);
}
app.UseRequestLocalization();
app.UseSession();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();

app.UseMiddleware<TokenValidationMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();