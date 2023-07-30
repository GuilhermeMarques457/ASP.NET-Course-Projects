using ContactsManager.Core.Domain.IdentityEntities;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryContracts;
using SampleApplicationCRUD.Filters.ActionFilters;
using SampleApplicationCRUD.Filters.ResultFilters;
using Serilog;
using ServiceContracts;
using ServiceContracts.IPersonService;
using Services.CountriesService;
using Services.PeopleService;

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
            services.AddScoped<ICountriesUploaderService, CountriesUploaderService>();
            services.AddScoped<ICountriesAdderService, CountriesAdderService>();
            services.AddScoped<ICountriesGetterService, CountriesGetterService>();
            services.AddScoped<IPersonAdderService, PersonAdderService>();
            services.AddScoped<IPersonUpdaterService, PersonUpdaterService>();
            services.AddScoped<IPersonDeleterService, PersonDeleterService>();
            services.AddScoped<IPersonSorterService, PersonSorterService>();
            services.AddScoped<IPersonGetterService, PersonGetterServiceWithFewExcelFields>();

            // We are not using this child implementation now because using this we can violate the LSP principle
            // Using the interface new creation is better like I did above
            //services.AddScoped<IPersonGetterService, PersonGetterServiceChild>();

            // Adding the service so we can access it in the changed method
            // We wanted to change the method so we created a new one because of Open/Closed Principle
            services.AddScoped<PersonGetterService, PersonGetterService>();

            // Ading the repository with dependency injection with inversion of control (creating a Ioc container)
            services.AddScoped<IPeopleRepository, PeopleRepository>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();

            //Adding just the filter as a service with dependecy injection
            services.AddTransient<PersonsListResultFilter>();
            services.AddTransient<ResponseHeaderActionFactoryFilter>();

            //By default AddDbContext is defined as a scoped service
            services.AddDbContext<AspNetDbContext>(options =>
            {
                options.UseSqlServer(configuration!.GetConnectionString("DefaultConnection"));
            });

            // Enable Identity in our project
            // <Table that we will store the user details, Table that will store the role details>
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 7;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredUniqueChars = 4;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AspNetDbContext>()
            // Configurating the repository layer for manipulating the USER data
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AspNetDbContext, Guid>>()
            // Configurating the repository layer for manipulating the ROLE data
            .AddRoleStore<RoleStore<ApplicationRole, AspNetDbContext, Guid>>();

            services.AddAuthorization(options =>
            {
                // To access any action to the entire application the user must be loged in
                // Unless in the controllers or actions that you specify the attribute [AllowAnonymous]
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); //enforces authoriation policy (user must be authenticated) for all the action methods
            });

            // If user is not logged in, user will be redirect to this action 
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
            });

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;

            });

            return services;
        }
    }
}
