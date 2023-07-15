using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    //Accessing the program class from the main application
    //To do so we had to:
    //1. Add a partial class in the bottom of the program file
    //2. Add a statement that allows the tests to get the program in the project file (click right buttom in the project)
    //3. Install the package "Microsoft.AspNetCore.Mvc.Testing"


    //This class executes the program.cs file when a object is create because of the generic parameter
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                var descripter = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<AspNetDbContext>));

                if(descripter != null)
                {
                    //Removing the context service
                    services.Remove(descripter);
                }

                //Every time when we run the tests a new database is created
                services.AddDbContext<AspNetDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestingDB");
                });
            });
        }
    }
}
