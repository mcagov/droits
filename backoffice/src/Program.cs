using System.Globalization;
using Droits.Clients;
using Droits.Data;
using Droits.ModelBinders;
using Droits.Repositories;
using Droits.Services;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews(options =>
    {
        options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
    })
    .AddRazorRuntimeCompilation();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-GB");
});


builder.Services.AddDbContext<DroitsContext>(opt => opt.UseInMemoryDatabase("droits"));


builder.Services.AddHealthChecks();

builder.Services.AddScoped<ILetterRepository, LetterRepository>();
builder.Services.AddScoped<ILetterService, LetterService>();

builder.Services.AddScoped<IGovNotifyClient, GovNotifyClient>();

builder.Services.AddScoped<IDroitRepository, DroitRepository>();
builder.Services.AddScoped<IDroitService, DroitService>();

builder.Services.AddScoped<IWreckRepository, WreckRepository>();
builder.Services.AddScoped<IWreckService, WreckService>();

builder.Services.AddScoped<ISalvorRepository, SalvorRepository>();
builder.Services.AddScoped<ISalvorService, SalvorService>();

builder.Services.AddGovUkFrontend();


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


app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseRequestLocalization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();