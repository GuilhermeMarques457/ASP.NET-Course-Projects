using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContracts;
using Repository;
using Serilog;
using CRUDExample.Filters.ActionFilters;
using SampleApplicationCRUD.Filters.ResultFilters;
using SampleApplicationCRUD.Filters.ActionFilters;
using SampleApplicationCRUD;
using SampleApplicationCRUD.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Logging configurations
//builder.Host.ConfigureLogging(loggingProvider =>
//{
//    loggingProvider.ClearProviders();
//    loggingProvider.AddConsole();
//    loggingProvider.AddEventLog();
//});

//Setting Serilog up
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration) //Reading the appsettings
        .ReadFrom.Services(services); //Reads our service and make them available to serilog

});

// Passing the configuration through parameter to the ConfigureServiceExtension class
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Which route I want to redirect if there are errors
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

app.UseSerilogRequestLogging();


//Enable httplog on our application
app.UseHttpLogging();


if(builder.Environment.IsEnvironment("Test") == false)
{
    //To setup the rotativa library
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

app.UseStaticFiles(); 
app.UseRouting(); // Identitify action method on given route
app.UseAuthentication(); // Reading Identity Cookie
app.UseAuthorization(); // Validates access permissions of the user 
app.MapControllers(); // Execute the filter pipeline action + filters (Add endpoints middleware)

// Conventional Routing (Old way, MVC I remember good times T-T)
//app.UseEndpoints(endpoints =>
//{
//    // Admin/Home/Index
//    // Admin (will point to the same because of default value)
//    endpoints.MapControllerRoute(
//        name: "areas",
//        pattern: "{area:exists}/{controller=Home}/{action=Index}"
//        );

//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller}/{action}/{id?}"
//        );
//});

app.Run();


//To access the auto-generated program class programmatically,
//This always exists but now we're saying explicty,
//therefore we can use the class anywhere
public partial class Program { }
