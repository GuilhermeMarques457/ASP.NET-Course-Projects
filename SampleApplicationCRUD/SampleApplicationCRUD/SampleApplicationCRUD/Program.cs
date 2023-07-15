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
app.UseRouting();

// Add endpoints middleware
app.MapControllers();

app.Run();


//To access the auto-generated program class programmatically,
//This always exists but now we're saying explicty,
//therefore we can use the class anywhere
public partial class Program { }
