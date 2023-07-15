using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryContracts;
using SampleApplicationCRUD.Filters.ActionFilters;
using SampleApplicationCRUD.Filters.ResultFilters;
using Serilog;
using ServiceContracts;
using Services;

namespace SampleApplicationCRUD
{
    public static class ConfigureServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // We can do the same thing as we do in the program service method

            //To add a global filter
            services.AddControllersWithViews(options =>
            {
                //To add filter without parameters
                //options.Filters.Add<ResponseHeaderActionFilter>();

                //To get the ILogger Service
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

                //To add parametrized filter
                options.Filters.Add(new ResponseHeaderActionFilter("MyGlobalKey", "MyGlobalValue", 2));
            });
           
            //Adding the service with dependency injection with inversion of control (creating a Ioc container)
            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IPeopleRepository, PeopleRepository>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();

            //Adding just the service with dependecy injection
            services.AddTransient<PersonsListResultFilter>();
            services.AddTransient<ResponseHeaderActionFactoryFilter>();

            //By default AddDbContext is defined as a scoped service
            services.AddDbContext<AspNetDbContext>(options =>
            {
                options.UseSqlServer(configuration!.GetConnectionString("DefaultConnection"));
            });

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;

            });

            return services;
        }
    }
}
