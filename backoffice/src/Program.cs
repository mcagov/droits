using Droits.Clients;
using Droits.Models;
using Droits.Repositories;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<DroitsContext>(opt => opt.UseInMemoryDatabase("droits"));
builder.Services.AddHealthChecks();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IGovNotifyClient, GovNotifyClient>();

builder.Services.AddScoped<IDroitRepository, DroitRepository>();
builder.Services.AddScoped<IDroitService, DroitService>();


var app = builder.Build();

app.MapHealthChecks("/healthz");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();