using Microsoft.AspNetCore.Mvc.Authorization;
using ReactAdminNet;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers()
    .AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter()))
    .AddNewtonsoftJson();
services.AddMediatR();
services.AddCorsPolicy();
services.AddRetailServices(configuration);
services.AddAuthServices(configuration);
services.AddAuthorization();

// Configure app
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowOrigin");

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        defaults: new { controller = "Catchall", action = "Index" },
        pattern: "*");
});

await app.RunAsync();