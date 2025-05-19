using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Entities.Models;
using UserRepository;
using Microsoft.AspNetCore.Identity;

namespace UserService.Tests.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<UserRepositoryContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<UserRepositoryContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<UserRepositoryContext>();
                    db.Database.EnsureCreated();

                    var userManager = scopedServices.GetRequiredService<UserManager<User>>();

                    var user = new User
                    {
                        Id = "3b6e3995-056a-4c52-a65a-a5d419e23783",
                        UserName = "R0sale",
                        Email = "kvusov@bk.ru",
                        FirstName = "Kirill",
                        LastName = "Vusov",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(user, "Nika 2016").GetAwaiter().GetResult(); ;

                    if (!result.Succeeded)
                    {
                        throw new Exception("Failed to create test user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                    db.SaveChanges();
                }
            });
        }
    }
}
