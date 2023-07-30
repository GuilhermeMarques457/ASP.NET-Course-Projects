using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Test");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.Sources.Clear();

                var newConfiguration = new Dictionary<string, string?>() {
                        { "FinnhubToken", "ch1anbhr01qn6tg71hdgch1anbhr01qn6tg71he0" }
                    };

                config.AddInMemoryCollection(newConfiguration);
            });

            builder.ConfigureServices(services =>
            {
                var descripter = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<StockDbContext>));

                

                if (descripter != null)
                {
                    //Removing the context service
                    services.Remove(descripter);
                }

                //Every time when we run the tests a new database is created
                services.AddDbContext<StockDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestingDb");
                });

                builder.ConfigureAppConfiguration((WebHostBuilderContext ctx, Microsoft.Extensions.Configuration.IConfigurationBuilder config) =>
                {
                    
                });
            });
        }
    }
}
