using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryContracts;
using Serilog;
using Service;
using Service.FinnhubService;
using ServiceContracts;
using ServiceContracts.StockService;
using StocksApp.Core.Domain.RepositoryContracts;
using StocksApp.Infrastructure.Repositories;
using StocksApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration) //Reading the appsettings
        .ReadFrom.Services(services); //Reads our service and make them available to serilog

});

builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddScoped<IStockServiceBuyCreater, StockServiceBuyCreater>();
builder.Services.AddScoped<IStockServiceBuyGetter, StockServiceBuyGetter>();
builder.Services.AddScoped<IStockServiceSellCreater, StockServiceSellCreater>();
builder.Services.AddScoped<IStockServiceSellGetter, StockServiceSellGetter>();
builder.Services.AddScoped<IFinnhubServiceStockGetter, FinnhubServiceStockGetter>();
builder.Services.AddScoped<IFinnhubServiceStockPriceGetter, FinnhubServiceStockPriceGetter>();
builder.Services.AddScoped<IFinnhubServiceCompanyProfileGetter, FinnhubServiceCompanyProfileGetter>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IFinnhubRepository, FinnhubRepository>();

builder.Services.AddDbContext<StockDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;

});

builder.Services.AddHttpClient();


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlerMiddleware();
}

app.UseSerilogRequestLogging();

if (builder.Environment.IsEnvironment("Test") == false)
{
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

app.UseHttpLogging();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { }